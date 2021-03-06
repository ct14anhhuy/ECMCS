﻿using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;

namespace ECMCS.Route
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();

        private const int SW_HIDE = 0;

        private static MessageProvider _messageProvider;
        private static FileDownloader _fileDownloader;

        private static void Main(string[] args)
        {
            _messageProvider = new MessageProvider(RouteMessageContants.WINDOW_TITLE);
            _fileDownloader = new FileDownloader();

            ShowWindow(GetConsoleWindow(), SW_HIDE);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            RouteToAction(args[0]);
        }

        /// <summary>
        /// Global logs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ThreadContext.Properties["appName"] = "ECMCS.Route";
            LogHelper.Error((e.ExceptionObject as Exception).Message);
            Environment.Exit(1);
        }

        private static void RouteToAction(string args)
        {
            args = UrlHelper.Decode(args);
            string action = args.Extract("<", ">")[0];
            switch (action)
            {
                case RouteMessageContants.DOWNLOAD:
                    DownloadAction(args);
                    break;

                case RouteMessageContants.REDIRECT:
                    string epLiteId = GetUserFromDRMLog();
                    RedirectAction(epLiteId);
                    break;

                case RouteMessageContants.FILE_SHARE_URL:
                    args = args.Substring(args.LastIndexOf('>') + 1);
                    args = Encryptor.Decrypt(args);
                    _messageProvider.Send($"<{RouteMessageContants.FILE_SHARE_URL}>{args}");
                    break;

                case RouteMessageContants.LOGOUT:
                    _messageProvider.Send($"<{RouteMessageContants.LOGOUT}><//>");
                    break;

                default:
                    break;
            }
        }

        private static void DownloadAction(string args)
        {
            if (Process.GetProcessesByName("ECMCS.App").Length == 0)
            {
                return;
            }
            args = args.Substring(args.LastIndexOf('>') + 1);
            args = Encryptor.Decrypt(args);
            string decodedUrl = HttpUtility.UrlDecode(args);
            _fileDownloader.Download(decodedUrl);
        }

        private static void RedirectAction(string epLiteId)
        {
            if (Process.GetProcessesByName("ECMCS.App").Length == 0)
            {
                string path = Env.IS_DEVELOPMENT ? Assembly.GetExecutingAssembly().Location : Environment.ExpandEnvironmentVariables(@"%AppData%\Microsoft\Windows\Start Menu\Programs\Posco VST\ECM.appref-ms");
                Process.Start(path).Dispose();
            }
            string accessToken = GetToken(epLiteId);
            if (!string.IsNullOrEmpty(accessToken))
            {
                SaveUserLogin(epLiteId);
                string browser = Env.IS_DEVELOPMENT ? "chrome" : "firefox";
                string baseUrl = SystemParams.WEB_URL + "/Redirect/" + accessToken;
                Process.Start(browser, baseUrl).Dispose();
            }
        }

        private static string GetToken(string epLiteId)
        {
            string tokenUrl = $"{SystemParams.API_URL}/Token/GetToken?epLiteId=" + epLiteId;
            var client = new HttpClient();
            var response = client.GetAsync(tokenUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                string accessToken = Regex.Replace(result, "\\\"", "");
                return accessToken;
            }
            return string.Empty;
        }

        private static void SaveUserLogin(string epLiteId)
        {
            JsonHelper jsonHelper = new JsonHelper(CommonConstants.USER_FILE);
            var emp = new { epLiteId };
            var json = jsonHelper.Get<object>();
            if (json != null)
            {
                var currentUser = JsonConvert.DeserializeAnonymousType(json.FirstOrDefault().ToString(), emp);
                if (emp.epLiteId != currentUser.epLiteId)
                {
                    jsonHelper.AddDefault<object>(emp);
                }
            }
            else
            {
                jsonHelper.AddDefault<object>(emp);
            }
        }

        private static string GetUserFromDRMLog()
        {
            string currentUser = "";
            string drmLogFile = Environment.Is64BitOperatingSystem ? SystemParams.DRM_LOG_FILE_X64 : SystemParams.DRM_LOG_FILE_X32;
            var fileContents = File.ReadLines(drmLogFile).Reverse();
            foreach (var line in fileContents)
            {
                if (line.Contains("UserID = (null)") || line.Contains("InitInstance") || line.Contains("Status = LOGOUT"))
                {
                    throw new Exception("You are not logged in");
                }
                if (line.Contains("UserID") && line.Contains("DomainID = 0000000000012514"))
                {
                    currentUser = line.Trim().Substring(line.LastIndexOf(" ") + 1);
                    break;
                }
            }
            return currentUser;
        }
    }
}
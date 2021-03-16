using ECMCS.Utilities;
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

        private static void Main(string[] args)
        {
            try
            {
                //ShowWindow(GetConsoleWindow(), SW_HIDE);
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                string encodedUrl = HttpUtility.UrlDecode(args[0]);
                string urlParams = UrlHelper.Decode(encodedUrl.Substring(args[0].IndexOf(':') + 1)).Trim().Replace(" ", "+");
                RouteToAction(urlParams);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
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
                case "Download":
                    DownloadAction(args);
                    break;

                case "Redirect":
                    string epLiteId = ReadDRMLog();
                    RedirectAction(epLiteId);
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
            FileDownloader downloader = new FileDownloader(args);
            downloader.Download();
        }

        private static void RedirectAction(string epLiteId)
        {
            if (Process.GetProcessesByName("ECMCS.App").Length == 0)
            {
                if (Debugger.IsAttached)
                {
                    Process.Start(Assembly.GetExecutingAssembly().Location).Dispose();
                }
                else
                {
                    Process.Start($@"{Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).CodeBase)}\ECMCS.App.exe").Dispose();
                }
            }
            string accessToken = GetToken(epLiteId);
            if (!string.IsNullOrEmpty(accessToken))
            {
                InitData(epLiteId);
                string baseUrl = SystemParams.WEB_URL + "/AuthGate?token=" + accessToken;
                try
                {
                    Process.Start("chrome", baseUrl).Dispose();
                }
                catch
                {
                    Process.Start($"microsoft-edge:{baseUrl}").Dispose();
                }
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

        private static void InitData(string epLiteId)
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

        private static string ReadDRMLog()
        {
            string currentUser = "";
            string drmLogFile = Environment.Is64BitOperatingSystem ? SystemParams.DRM_LOG_FILE_X64 : SystemParams.DRM_LOG_FILE_X32;
            foreach (var line in File.ReadLines(drmLogFile).Reverse())
            {
                if (line.Contains("UserID"))
                {
                    if (CheckLoggedIn(line))
                    {
                        throw new Exception("You are not logged in");
                    }
                    currentUser = line.Trim().Substring(line.LastIndexOf(" ") + 1);
                    break;
                }
            }
            return currentUser;
        }

        private static bool CheckLoggedIn(string line)
        {
            return line.Contains("UserID = (null)") || line.Contains("InitInstance");
        }
    }
}
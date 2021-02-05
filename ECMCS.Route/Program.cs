using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using log4net;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            string urlParams = UrlHelper.Decode(args[0].Substring(args[0].IndexOf(':') + 1)).Trim().Replace(" ", "+");
            RouteToAction(urlParams);
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
                    string epLiteId = args.Extract("[", "]")[0];
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
                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}ECMCS.App.exe").Dispose();
            }
            string accessToken = GetToken(epLiteId);
            if (!string.IsNullOrEmpty(accessToken))
            {
                InitData(epLiteId);
                string baseUrl = ConfigHelper.Read("WebUrl") + "/AuthGate?token=" + accessToken;
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
            string tokenUrl = $"{ConfigHelper.Read("ApiUrl")}/Token/GetToken?epLiteId=" + epLiteId;
            var client = new HttpClient();
            var response = client.GetAsync(tokenUrl).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string accessToken = Regex.Replace(result, "\\\"", "");
            return accessToken;
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
    }
}
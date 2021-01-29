﻿using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;

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
            string urlParams = UrlHelper.Decode(args[0].Substring(args[0].IndexOf(':') + 1)).Trim().Replace(" ", "+");
            RouteToAction(urlParams);
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
            string empId = WebUtility.UrlEncode(epLiteId);
            string baseUrl = ConfigHelper.Read("WebUrl") + "/AuthGate?empId=" + empId;
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
}
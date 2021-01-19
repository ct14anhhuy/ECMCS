﻿using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.Diagnostics;
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
            string urlParams = UrlHelper.Decode(args[0].Substring(args[0].LastIndexOf(':') + 1)).Trim().Replace(" ", "+");
            string decrypted = Encryptor.Decrypt(urlParams);
            RouteToAction(decrypted);
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
                    RedirectAction();
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

        private static void RedirectAction()
        {
            if (Process.GetProcessesByName("ECMCS.App").Length == 0)
            {
                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}ECMCS.App.exe").Dispose();
            }
            string baseUrl = ConfigHelper.Read("WebUrl");
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
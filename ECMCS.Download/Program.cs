using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ECMCS.Download
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
            if (Process.GetProcessesByName("ECMCS.App").Length == 0)
            {
                Process.Start($"{AppDomain.CurrentDomain.BaseDirectory}ECMCS.App.exe");
            }
            FileDownloader downloader = new FileDownloader(args[0]);
            downloader.Download();
            Console.ReadLine();
        }
    }
}
using System;
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
            FileDownloader downloader = new FileDownloader(args[0]);
            downloader.Download();
        }
    }
}
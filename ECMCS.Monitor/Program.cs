using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ECMCS.Monitor
{
    internal class Program
    {
        private static string[] extensions = { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };

        [STAThread]
        private static void Main(string[] args)
        {
            if (PriorProcess() != null)
            {
                return;
            }
            Watch(ConfigurationManager.AppSettings["SaveFilePath"]);
            Console.ReadLine();
        }

        private static void Watch(string path)
        {
            Console.WriteLine("ECM listening...");
            var watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            watcher.Filter = "*.*";
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.Renamed += Watcher_Renamed;
            watcher.Deleted += Watcher_Deleted;
            watcher.Changed += Watcher_Changed;
            watcher.Created += Watcher_Created;
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (extensions.Any(ext.Equals))
            {
                Console.WriteLine($"{e.FullPath} created");
            }
        }

        private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (extensions.Any(ext.Equals))
            {
                Console.WriteLine($"{e.FullPath} changed");
            }
        }

        private static void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (extensions.Any(ext.Equals))
            {
                Console.WriteLine($"{e.FullPath} deleted");
            }
        }

        private static void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (extensions.Any(ext.Equals))
            {
                Console.WriteLine($"{e.OldFullPath} renamed to {e.FullPath}");
            }
        }

        public static Process PriorProcess()
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }
    }
}
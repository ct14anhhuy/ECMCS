using System;
using System.Diagnostics;
using System.IO;

namespace ECMCS.Utilities
{
    public static class FileHelper
    {
        public static void OpenFile(string path)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = "\"" + path + "\"";
                process.Start();
            }
        }

        public static string CreatePath(string path, string subPath)
        {
            string fullPath = path + subPath;
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return fullPath;
        }

        public static void CreatePath(string path, params string[] subPaths)
        {
            for (int i = 0; i < subPaths.Length; i++)
            {
                string fullPath = path + subPaths[i];
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
            }
        }

        public static void CreateFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Dispose();
            }
        }

        public static void SetHiddenFolder(string path, bool isHidden)
        {
            try
            {
                string command = isHidden ? $"attrib +s +h {path}" : $"attrib -s -h {path}";
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command);
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = true;
                using (Process proc = new Process())
                {
                    proc.StartInfo = procStartInfo;
                    proc.Start();
                    proc.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Empty(params string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                DirectoryInfo directory = new DirectoryInfo(paths[i]);
                foreach (FileInfo file in directory.EnumerateFiles())
                {
                    if (file.CreationTime < DateTime.Today)
                    {
                        file.Delete();
                    }
                }
                foreach (DirectoryInfo dir in directory.EnumerateDirectories())
                {
                    if (dir.CreationTime < DateTime.Today)
                    {
                        dir.Delete(true);
                    }
                }
            }
        }
    }
}
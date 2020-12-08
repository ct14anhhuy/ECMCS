using System;
using System.Diagnostics;
using System.IO;

namespace ECMCS.Utilities
{
    public static class FileHelper
    {
        public static bool CheckFileLocked(string fileName)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (UnauthorizedAccessException)
            {
                try
                {
                    fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (Exception)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return false;
        }

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
            DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            return fullPath;
        }

        public static void CreatePath(string path, params string[] subPaths)
        {
            if (subPaths.Length == 0)
            {
                throw new ArgumentNullException($"{nameof(subPaths)} can not null");
            }
            for (int i = 0; i < subPaths.Length; i++)
            {
                string fullPath = path + subPaths[i];
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
            }
        }

        public static void CreateFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                fileInfo.Create();
            }
        }
    }
}
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

        public static void OpenFileWithDefaultProgram(string path)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = "\"" + path + "\"";
                process.Start();
            }
        }

        public static string CreateSubPath(string path, string subPath)
        {
            string fullPath = path + subPath;
            bool exists = Directory.Exists(path + subPath);
            if (!exists)
            {
                Directory.CreateDirectory(path + subPath);
            }
            return fullPath;
        }
    }
}
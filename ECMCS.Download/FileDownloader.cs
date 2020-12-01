using ECMCS.DTO;
using ECMCS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ECMCS.Download
{
    public class FileDownloader
    {
        private string _path = ConfigurationManager.AppSettings["SaveFilePath"];
        private string _url;

        public FileDownloader(string url)
        {
            _url = url;
        }

        public void Download()
        {
            string subPath = $@"ECM{DateTime.Now:ddMMyyyyHHmmssfff}\";
            var fileInfo = ExtractFromString(_url, "[", "]");
            WebClient client = new WebClient();
            fileInfo.FilePath = FileHelper.CreateSubPath(_path, subPath) + Path.GetFileName(fileInfo.Url);
            client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
            FileHelper.OpenFileWithDefaultProgram(fileInfo.FilePath);
            JsonHelper.AddObjectsToJson(ConfigurationManager.AppSettings["JsonFilePath"], fileInfo);
        }

        private FileInfoDTO ExtractFromString(string source, string start, string end)
        {
            FileInfoDTO fileInfo = new FileInfoDTO();
            var strs = new List<string>();
            string pattern = string.Format("{0}({1}){2}", Regex.Escape(start), ".+?", Regex.Escape(end));
            foreach (Match m in Regex.Matches(source, pattern))
            {
                strs.Add(m.Groups[1].Value);
            }
            fileInfo.Url = strs[0].Replace("%5C", @"\");
            fileInfo.Owner = strs[1];
            fileInfo.Version = strs[2];
            return fileInfo;
        }
    }
}
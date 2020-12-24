using ECMCS.DTO;
using ECMCS.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ECMCS.Download
{
    public class FileDownloader
    {
        private readonly string _path = ConfigHelper.Read("SaveFilePath.Root") + ConfigHelper.Read("SaveFilePath.Monitor");
        private readonly string _url;

        public FileDownloader(string url)
        {
            _url = url;
        }

        public void Download()
        {
            string subPath = $@"ECM{DateTime.Now:ddMMyyyyHHmmssfff}\";
            var fileInfo = ExtractFromString(_url, "[", "]");
            using (WebClient client = new WebClient())
            {
                fileInfo.FilePath = FileHelper.CreatePath(_path, subPath) + Path.GetFileName(fileInfo.Url);
                client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
                FileHelper.OpenFile(fileInfo.FilePath);
                JsonHelper.Add(fileInfo);
            }
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
            fileInfo.Id = strs[0];
            fileInfo.Url = strs[1].Replace("%5C", @"\");
            fileInfo.FileName = Path.GetFileName(fileInfo.Url);
            fileInfo.Owner = strs[2];
            fileInfo.Version = strs[3];
            return fileInfo;
        }
    }
}
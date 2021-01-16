using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.IO;
using System.Net;

namespace ECMCS.Route
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
            var fileInfo = ExtractParamsFromUrl(_url, "[", "]");
            using (WebClient client = new WebClient())
            {
                fileInfo.FilePath = FileHelper.CreatePath(_path, subPath) + Path.GetFileName(fileInfo.Url);
                Console.WriteLine(fileInfo.Url);
                Console.WriteLine(fileInfo.FilePath);
                client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
                FileHelper.OpenFile(fileInfo.FilePath);
                JsonHelper.Add(fileInfo);
            }
        }
        private FileDownloadDTO ExtractParamsFromUrl(string source, string start, string end)
        {
            FileDownloadDTO fileInfo = new FileDownloadDTO();
            string[] extractedStr = source.Extract(start, end);
            fileInfo.Id = int.Parse(extractedStr[0]);
            fileInfo.Url = extractedStr[1];
            fileInfo.FileName = Path.GetFileName(fileInfo.Url);
            fileInfo.Owner = extractedStr[2];
            fileInfo.Version = extractedStr[3];
            fileInfo.ReadOnly = bool.Parse(extractedStr[4]);
            return fileInfo;
        }
    }
}
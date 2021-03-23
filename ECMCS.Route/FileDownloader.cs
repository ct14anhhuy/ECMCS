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
        private readonly string _path = SystemParams.FILE_PATH_ROOT + SystemParams.FILE_PATH_MONITOR;
        private readonly JsonHelper _jsonHelper;
        private readonly string _url;

        public FileDownloader(string url)
        {
            _url = url;
            _jsonHelper = new JsonHelper();
        }

        public void Download()
        {
            string subPath = $@"ECM{DateTime.Now:ddMMyyyyHHmmssfff}\";
            var fileInfo = ExtractParamsFromUrl(_url, "[", "]");
            using (WebClient client = new WebClient())
            {
                Console.WriteLine(fileInfo.Url);
                fileInfo.FilePath = FileHelper.CreatePath(_path, subPath) + Path.GetFileName(fileInfo.Url);
                client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
                fileInfo.FileSize = File.ReadAllBytes(fileInfo.FilePath).Length;
                FileHelper.OpenFile(fileInfo.FilePath);
                _jsonHelper.Add(fileInfo);
            }
        }

        private FileDownloadDTO ExtractParamsFromUrl(string source, string start, string end)
        {
            string[] extractedStr = source.Extract(start, end);
            FileDownloadDTO fileInfo = new FileDownloadDTO();
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
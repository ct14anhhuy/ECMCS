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

        public FileDownloader()
        {
            _jsonHelper = new JsonHelper();
        }

        public void Download(string url)
        {
            string subPath = $@"ECM{DateTime.Now:ddMMyyyyHHmmssfff}\";
            var fileInfo = ExtractParamsFromUrl(url, "</", "/>");
            using (WebClient client = new WebClient())
            {
                fileInfo.FilePath = FileHelper.CreatePath(_path, subPath) + Path.GetFileName(fileInfo.Url);
                _jsonHelper.Add(fileInfo);
                client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
                FileHelper.OpenFile(fileInfo.FilePath);
            }
        }

        private FileDownloadDTO ExtractParamsFromUrl(string source, string start, string end)
        {
            string[] extractedStr = source.Extract(start, end);
            FileDownloadDTO fileInfo = new FileDownloadDTO
            {
                Id = Guid.Parse(extractedStr[0]),
                Url = extractedStr[1],
                Owner = extractedStr[2],
                Version = extractedStr[3],
                ReadOnly = bool.Parse(extractedStr[4])
            };
            fileInfo.FileName = Path.GetFileName(fileInfo.Url);
            return fileInfo;
        }
    }
}
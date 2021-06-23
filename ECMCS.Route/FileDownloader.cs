using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace ECMCS.Route
{
    public class FileDownloader
    {
        private readonly string _path = SystemParams.FILE_PATH_ROOT + SystemParams.FILE_PATH_MONITOR;
        private readonly JsonHelper _jsonHelper;
        private readonly MessageProvider _messageProvider;

        public FileDownloader()
        {
            _jsonHelper = new JsonHelper();
            _messageProvider = new MessageProvider("ECMCS");
        }

        public void Download(string url)
        {
            string subPath = $@"ECM{DateTime.Now:ddMMyyyyHHmmssfff}\";
            var fileInfo = ExtractParamsFromUrl(url, "</", "/>");
            string path = FileHelper.CreatePath(_path, subPath);
            string fileName = Path.GetFileName(fileInfo.Url);
            using (WebClient client = new WebClient())
            {
                fileInfo.FilePath = path + fileName;
                _jsonHelper.Add(fileInfo);
                client.DownloadFile(fileInfo.Url, fileInfo.FilePath);
                FileHelper.OpenFile(fileInfo.FilePath);
                WaitFileOpened(fileInfo.FilePath);
            }
        }

        private void WaitFileOpened(string filePath)
        {
            int checkTimeout = 0;
            while (true)
            {
                if (FileHelper.CheckFileLocked(filePath))
                {
                    break;
                }
                if (checkTimeout >= 10)
                {
                    throw new TimeoutException("Can not open this file");
                }
                Thread.Sleep(1000);
                checkTimeout++;
            }
            _messageProvider.Send($"<{RouteMessageContants.FILE_OPENED}></{Encryptor.Encrypt(filePath)}/>");
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
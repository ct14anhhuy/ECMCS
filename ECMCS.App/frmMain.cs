using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ECMCS.App
{
    public delegate void SendMessenge<T>(T entity);

    public partial class frmMain : Form
    {
        private readonly string[] _extensions = { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };
        private readonly string _monitorPath = ConfigHelper.Read("SaveFilePath.Root") + ConfigHelper.Read("SaveFilePath.Monitor");
        private readonly string _syncPath = ConfigHelper.Read("SyncFilePath");
        private readonly string _protocolName = ConfigHelper.Read("Protocol.Name");
        private readonly string _routeAppPath = $"{AppDomain.CurrentDomain.BaseDirectory}ECMCS.Route.exe";
        private readonly JsonHelper _jsonHelper;
        private string _epLiteId;
        private int _fireCount = 0;

        public frmMain()
        {
            InitializeComponent();
            ProtocolHelper.Create(_protocolName, _routeAppPath);
            CreateResources();
            ShowBalloonTip("Info", "ECM is running", ToolTipIcon.Info);

            MonitorWatcher(_monitorPath);
            SyncWatcher(_syncPath);
            _jsonHelper = new JsonHelper();
        }

        private void CreateResources()
        {
            string rootPath = ConfigHelper.Read("SaveFilePath.Root");
            string monitorPath = ConfigHelper.Read("SaveFilePath.Monitor");
            string logPath = ConfigHelper.Read("SaveFilePath.Log");
            string jsonFile = ConfigHelper.Read("JsonFileName.Files");
            string usersFile = ConfigHelper.Read("JsonFileName.Users");
            FileHelper.CreatePath(rootPath, monitorPath, logPath);
            FileHelper.SetHiddenFolder(rootPath.TrimEnd('\\'), false);
            FileHelper.Empty($"{rootPath}{monitorPath}");
            FileHelper.CreateFile($"{rootPath}{monitorPath}", jsonFile, usersFile);
            FileHelper.CreatePath(_syncPath);
            CreateFolderIcon(_syncPath);
        }

        private void CreateFolderIcon(string folderPath)
        {
            string iconPath = Directory.GetCurrentDirectory() + @"\Resources\icon-ecm-colored.ico";
            FolderIcon folderIcon = new FolderIcon(folderPath);
            folderIcon.CreateFolderIcon(iconPath, "Sync To ECM");
        }

        private void ShowBalloonTip(string title, string messenge, ToolTipIcon icon)
        {
            notifyIcon.BalloonTipIcon = icon;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = messenge;
            notifyIcon.ShowBalloonTip(1000);
        }

        private void OpenUpdateFrm(FileDownloadDTO fileDownload)
        {
            var frm = new frmUpdateVersion();
            var delSendMsg = new SendMessenge<FileDownloadDTO>(frm.EventListener);
            delSendMsg(fileDownload);
            frm.OnUploadClosed += Frm_OnUploadClosed;
            frm.Show();
        }

        private void Frm_OnUploadClosed(object sender, FileDownloadDTO e)
        {
            if (e.IsUploaded)
            {
                ShowBalloonTip("Info", $"Upload file successfully", ToolTipIcon.Info);
            }
            else
            {
                ShowBalloonTip("Info", $"File upload failed", ToolTipIcon.Warning);
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated && value)
            {
                base.CreateHandle();
                value = false;
            }
            base.SetVisibleCore(value);
        }

        private void exitECMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        #region Monitor watcher

        private void MonitorWatcher(string path)
        {
            var monitorWatcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                SynchronizingObject = this
            };
            monitorWatcher.Deleted += MonitorWatcher_Deleted;
            monitorWatcher.Changed += MonitorWatcher_Changed;
        }

        private void MonitorWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _fireCount++;
            if (_fireCount == 1)
            {
                if (e.Name == ConfigHelper.Read("JsonFileName.Users"))
                {
                    JsonHelper jsonHelper = new JsonHelper(CommonConstants.USER_FILE);
                    var json = jsonHelper.Get<object>().FirstOrDefault();
                    var emp = JsonConvert.DeserializeAnonymousType(json.ToString(), new { epLiteId = "" });
                    _epLiteId = emp.epLiteId;
                    ShowBalloonTip("Login Success", "Current user: " + emp.epLiteId, ToolTipIcon.Info);
                    LogHelper.Info("Logged on: " + emp.epLiteId);
                }
            }
            else
            {
                _fireCount = 0;
            }
        }

        private void MonitorWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_extensions.Any(ext.Equals))
            {
                string subPath = Path.GetDirectoryName(e.FullPath);
                var fileDownload = _jsonHelper.Get<FileDownloadDTO>(x => x.FilePath.Contains(subPath) && !x.IsDone).FirstOrDefault();
                fileDownload.Modifier = _epLiteId;
                if (fileDownload != null && !fileDownload.ReadOnly)
                {
                    fileDownload.IsDone = true;
                    _jsonHelper.Update(fileDownload, x => x.FilePath == fileDownload.FilePath);
                    OpenUpdateFrm(fileDownload);
                }
            }
        }

        #endregion Monitor watcher

        #region Sync watcher

        private void SyncWatcher(string path)
        {
            var syncWatcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.*",
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                SynchronizingObject = this
            };
            syncWatcher.Created += SyncWatcher_Created;
        }

        private void SyncWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_extensions.Any(ext.Equals))
            {
                frmSyncToECM frm = new frmSyncToECM();
                frm.OnUploadClosed += Frm_OnUploadClosedSync;
                var delSendMsg = new SendMessenge<(string, string)>(frm.EventListener);
                delSendMsg((e.FullPath, _epLiteId));
                frm.Show();
            }
        }

        private void Frm_OnUploadClosedSync(object sender, bool e)
        {
            if (e)
            {
                ShowBalloonTip("Info", $"Upload file successfully", ToolTipIcon.Info);
            }
            else
            {
                ShowBalloonTip("Info", $"File upload failed", ToolTipIcon.Warning);
            }
        }

        #endregion Sync watcher
    }
}
using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
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

        public frmMain()
        {
            InitializeComponent();
            ProtocolHelper.Create(_protocolName, _routeAppPath);
            CreateResources();
            ShowBalloonTip("Info", "ECM is running", ToolTipIcon.Info);

            MonitorWatcher(_monitorPath);
            SyncWatcher(_syncPath);
        }

        private void CreateResources()
        {
            string rootPath = ConfigHelper.Read("SaveFilePath.Root");
            string monitorPath = ConfigHelper.Read("SaveFilePath.Monitor");
            string viewPath = ConfigHelper.Read("SaveFilePath.View");
            string jsonFileName = ConfigHelper.Read("JsonFileName");

            FileHelper.CreatePath(rootPath, monitorPath, viewPath);
            FileHelper.SetHiddenFolder(rootPath.TrimEnd('\\'), false);
            FileHelper.Empty($"{rootPath}{monitorPath}", $"{rootPath}{viewPath}");
            FileHelper.CreateFile($"{rootPath}{monitorPath}{jsonFileName}");

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

        private void OpenUpdateFrm(FileInfoDTO fileInfo)
        {
            var frm = new frmUpdateVersion();
            var delSendMsg = new SendMessenge<FileInfoDTO>(frm.EventListener);
            delSendMsg(fileInfo);
            frm.OnUploadClosed += Frm_OnUploadClosed;
            frm.FormClosed += Frm_FormClosed;
            frm.Show();
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MemoryManagement.FlushMemory();
        }

        private void Frm_OnUploadClosed(object sender, FileInfoDTO e)
        {
            if (e.IsUploaded)
            {
                ShowBalloonTip("Info", $"File {e.FileName} successfully uploaded", ToolTipIcon.Info);
            }
            else
            {
                ShowBalloonTip("Info", $"File {e.FileName} has not uploaded", ToolTipIcon.Warning);
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
            monitorWatcher.Deleted += MonitorWatcher_Deleted; ;
        }

        private void MonitorWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_extensions.Any(ext.Equals))
            {
                string subPath = Path.GetDirectoryName(e.FullPath);
                var fileInfo = JsonHelper.Get<FileInfoDTO>(x => x.FilePath.Contains(subPath) && !x.IsDone).FirstOrDefault();
                if (fileInfo != null && !fileInfo.ReadOnly)
                {
                    fileInfo.IsDone = true;
                    JsonHelper.Update(fileInfo, x => x.FilePath == fileInfo.FilePath);
                    OpenUpdateFrm(fileInfo);
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
                var delSendMsg = new SendMessenge<(string, string)>(frm.EventListener);
                delSendMsg((e.FullPath, ""));
                frm.Show();
            }
        }

        #endregion Sync watcher
    }
}
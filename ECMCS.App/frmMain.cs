using ECMCS.DTO;
using ECMCS.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ECMCS.App
{
    public delegate void SendMessenge<T>(T entity);

    public partial class frmMain : Form
    {
        private static string[] _extensions = { ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };
        private string _monitorPath = ConfigHelper.ReadSetting("SaveFilePath.Root") + ConfigHelper.ReadSetting("SaveFilePath.Monitor");

        public frmMain()
        {
            InitializeComponent();
            CreateResources();
            ShowBalloonTip("Info", "ECM is running", ToolTipIcon.Info);
            Watch(_monitorPath);
        }

        private static void CreateResources()
        {
            string rootPath = ConfigHelper.ReadSetting("SaveFilePath.Root");
            string monitorPath = ConfigHelper.ReadSetting("SaveFilePath.Monitor");
            string viewPath = ConfigHelper.ReadSetting("SaveFilePath.View");
            string jsonFileName = ConfigHelper.ReadSetting("JsonFileName");
            FileHelper.CreatePath(rootPath, monitorPath, viewPath);
            FileHelper.CreateFile($"{rootPath}{monitorPath}{jsonFileName}");
        }

        private void ShowBalloonTip(string title, string messenge, ToolTipIcon icon)
        {
            notifyIcon.BalloonTipIcon = icon;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = messenge;
            notifyIcon.ShowBalloonTip(3000);
        }

        private void Watch(string path)
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";
            watcher.EnableRaisingEvents = true;
            watcher.IncludeSubdirectories = true;
            watcher.Deleted += Watcher_Deleted;
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            var ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_extensions.Any(ext.Equals))
            {
                string subPath = Path.GetDirectoryName(e.FullPath);
                var fileInfo = JsonHelper.Get<FileInfoDTO>(x => x.FilePath.Contains(subPath) && !x.IsDone).SingleOrDefault();
                if (fileInfo != null)
                {
                    fileInfo.IsDone = true;
                    JsonHelper.Update(fileInfo, x => x.FilePath == fileInfo.FilePath);
                    OpenUpdateFrm(fileInfo);
                }
            }
        }

        private void OpenUpdateFrm(FileInfoDTO fileInfo)
        {
            var frm = new frmUpdateVersion();
            var delSendMsg = new SendMessenge<FileInfoDTO>(frm.EventListener);
            delSendMsg(fileInfo);
            frm.OnUploadClosed += Frm_OnUploadClosed;
            frm.ShowDialog();
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
    }
}
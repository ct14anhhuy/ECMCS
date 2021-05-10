using ECMCS.App.Tracking;
using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ECMCS.App
{
    public delegate void SendMessenge<T>(T entity);

    public delegate void TrackingFile(string fullPath);

    public partial class frmMain : Form
    {
        private readonly string[] _fileTrackingExtensions = { ".doc", ".docx", ".xls", ".xlsx", ".xlsm", ".csv", ".ppt", ".pptx", ".pdf" };
        private readonly string _monitorPath = SystemParams.FILE_PATH_ROOT + SystemParams.FILE_PATH_MONITOR;
        private readonly string _routeAppPath = $@"{Path.GetDirectoryName(Application.ExecutablePath)}\ECMCS.Route.exe";
        private readonly ObservableCollection<FileChangeTracking> _queue;
        private readonly JsonHelper _jsonHelper;
        private string _epLiteId;
        private int _fireCount = 0;

        public frmMain()
        {
            InitializeComponent();
            ProtocolHelper.Create(SystemParams.PROTOCOL_NAME, _routeAppPath);
            CreateResources();
            ShowBalloonTip("Info", "ECM is running", ToolTipIcon.Info);
            MonitorWatcher(_monitorPath);
            SyncWatcher(SystemParams.SYNC_FILE_PATH);
            _jsonHelper = new JsonHelper();
            _epLiteId = GetCurrentUser();

            _queue = new ObservableCollection<FileChangeTracking>();
            _queue.CollectionChanged += Events_CollectionChanged;
        }

        private void Events_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    (item as FileChangeTracking).StartFileProcessing();
                }
            }
        }

        private void CreateResources()
        {
            FileHelper.CreatePath(SystemParams.FILE_PATH_ROOT, SystemParams.FILE_PATH_MONITOR, SystemParams.FILE_PATH_LOG);
            FileHelper.SetHiddenFolder(SystemParams.FILE_PATH_ROOT.TrimEnd('\\'), false);
            FileHelper.Empty($"{SystemParams.FILE_PATH_ROOT}{SystemParams.FILE_PATH_MONITOR}");
            FileHelper.CreateFile($"{SystemParams.FILE_PATH_ROOT}{SystemParams.FILE_PATH_MONITOR}", SystemParams.JSON_FILES, SystemParams.JSON_USERS);
            FileHelper.CreatePath(SystemParams.SYNC_FILE_PATH);
            CreateFolderIcon(SystemParams.SYNC_FILE_PATH);
        }

        private void CreateFolderIcon(string folderPath)
        {
            string iconPath = Directory.GetCurrentDirectory() + @"\Resources\icon-ecm-colored.ico";
            FolderIcon folderIcon = new FolderIcon(folderPath);
            folderIcon.CreateFolderIcon(iconPath, "Sync To ECM");
        }

        private void ShowBalloonTip(string title, string messenge, ToolTipIcon icon)
        {
            int timeout = 1000;
            notifyIcon.BalloonTipIcon = icon;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = messenge;
            notifyIcon.ShowBalloonTip(timeout);
        }

        private void OpenUpdateFrm(FileDownloadDTO fileDownload)
        {
            var frm = new frmUpdateVersion
            {
                TopMost = true
            };
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
            monitorWatcher.Created += MonitorWatcher_Created;
            monitorWatcher.Changed += MonitorWatcher_Changed;
        }

        private void MonitorWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_fileTrackingExtensions.Any(ext.Equals))
            {
                if (!Path.GetFileName(e.FullPath).Contains("~$"))
                {
                    FileChangeTracking fileCreate = new FileChangeTracking(this, e.FullPath);
                    _queue.Add(fileCreate);
                }
            }
        }

        private void MonitorWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            _fireCount++;
            if (_fireCount == 1)
            {
                if (e.Name == SystemParams.JSON_USERS)
                {
                    _epLiteId = GetCurrentUser();
                    ShowBalloonTip("Login Success", "Current user: " + _epLiteId, ToolTipIcon.Info);
                    LogHelper.Info("Logged on: " + _epLiteId);
                }
            }
            else
            {
                _fireCount = 0;
            }
        }

        private string GetCurrentUser()
        {
            JsonHelper jsonHelper = new JsonHelper(CommonConstants.USER_FILE);
            if (jsonHelper.Get<object>() != null)
            {
                var json = jsonHelper.Get<object>().FirstOrDefault();
                var emp = JsonConvert.DeserializeAnonymousType(json.ToString(), new { epLiteId = "" });
                return emp.epLiteId;
            }
            return null;
        }

        public void FileClosed(string fullPath)
        {
            string ext = (Path.GetExtension(fullPath) ?? string.Empty).ToLower();
            if (_fileTrackingExtensions.Any(ext.Equals))
            {
                string subPath = Path.GetDirectoryName(fullPath);
                var fileDownload = _jsonHelper.Get<FileDownloadDTO>(x => x.FilePath.Contains(subPath) && !x.IsDone).FirstOrDefault();
                if (IsModifiedFile(fileDownload.FilePath))
                {
                    fileDownload.Modifier = _epLiteId;
                    if (fileDownload != null && !fileDownload.ReadOnly)
                    {
                        fileDownload.IsDone = true;
                        _jsonHelper.Update(fileDownload, x => x.FilePath == fileDownload.FilePath);
                        OpenUpdateFrm(fileDownload);
                    }
                }
            }
        }

        private bool IsModifiedFile(string filePath)
        {
            DateTime creation = File.GetCreationTime(filePath);
            DateTime modification = File.GetLastWriteTime(filePath);
            if ((modification - creation).TotalSeconds > 1)
            {
                return true;
            }
            return false;
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
            string ext = (Path.GetExtension(e.FullPath) ?? string.Empty).ToLower();
            if (_fileTrackingExtensions.Any(ext.Equals))
            {
                frmSyncToECM frm = new frmSyncToECM
                {
                    TopMost = true
                };
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

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == NativeMethods.WM_COPYDATA)
            {
                NativeMethods.COPYDATASTRUCT copyData = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(message.LParam, typeof(NativeMethods.COPYDATASTRUCT));
                int dataType = (int)copyData.dwData;
                if (dataType == 2)
                {
                    string data = Marshal.PtrToStringAnsi(copyData.lpData);
                    string action = data.Extract("<", ">")[0];

                    switch (action)
                    {
                        case "FileShareUrl":
                            data = data.Substring(data.LastIndexOf('>') + 1);
                            data = Encryptor.Decrypt(data);
                            data = data.Extract("</", "/>")[0];
                            frmDownloadFileShare frm = new frmDownloadFileShare();
                            var delSendMsg = new SendMessenge<(string, int)>(frm.EventListener);
                            delSendMsg((_epLiteId, int.Parse(data)));
                            frm.Show();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception(string.Format("Unrecognized data type = {0}.", dataType));
                }
            }
            else
            {
                base.WndProc(ref message);
            }
        }
    }
}
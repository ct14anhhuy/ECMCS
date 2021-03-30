using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.Windows.Forms;

namespace ECMCS.App.Tracking
{
    public class FileChangeTracking
    {
        private const int TRACKING_INTERVAL = 3000;
        private readonly string _filePath;
        private readonly frmMain _sender;
        private Timer _processTimer;

        public FileChangeTracking(frmMain sender, string filePath)
        {
            _filePath = filePath;
            _sender = sender;
        }

        public void StartFileProcessing()
        {
            if (FileHelper.IsFileLocked(_filePath))
            {
                _processTimer = new Timer();
                _processTimer.Interval = TRACKING_INTERVAL;
                _processTimer.Tick += _processTimer_Tick;
                _processTimer.Enabled = true;
                _processTimer.Start();
            }
            else
            {
                ProcessFile();
            }
        }

        private void _processTimer_Tick(object sender, EventArgs e)
        {
            if (!FileHelper.IsFileLocked(_filePath))
            {
                _processTimer.Enabled = false;
                _processTimer.Stop();
                _processTimer.Dispose();
                ProcessFile();
            }
        }

        private void ProcessFile()
        {
            var trackingFile = new TrackingFile(_sender.FileClosed);
            trackingFile(_filePath);
        }
    }
}
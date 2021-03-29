using ECMCS.Utilities.FileFolderExtensions;
using System;
using System.Windows.Forms;

namespace ECMCS.App.Tracking
{
    public class FileChangeTracking
    {
        private readonly string _filePath;
        private readonly frmMain _sender;
        private const int TRACKING_INTERVAL = 3000;
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
                _processTimer.Tick += new EventHandler(processTimer_Elapsed);
                _processTimer.Enabled = true;
                _processTimer.Start();
            }
            else
            {
                ProcessFile();
            }
        }

        private void ProcessFile()
        {
            var trackingFile = new TrackingFile(_sender.FileClosed);
            trackingFile(_filePath);
        }

        private void processTimer_Elapsed(object sender, EventArgs e)
        {
            if (!FileHelper.IsFileLocked(_filePath))
            {
                _processTimer.Enabled = false;
                _processTimer.Stop();
                _processTimer.Dispose();

                ProcessFile();
            }
        }
    }
}
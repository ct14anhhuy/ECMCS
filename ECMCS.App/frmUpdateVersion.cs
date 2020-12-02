using ECMCS.DTO;
using ECMCS.Utilities;
using MetroFramework.Forms;
using System;

namespace ECMCS.App
{
    public partial class frmUpdateVersion : MetroForm
    {
        private FileInfoDTO _fileInfo;

        public frmUpdateVersion()
        {
            InitializeComponent();
        }

        private void frmUpdateVersion_Load(object sender, EventArgs e)
        {
            txtFileName.Enabled = false;
            txtCurrentVersion.Enabled = false;
            txtNextVersion.Enabled = false;
            rdUpdateNextVersion.Checked = true;

            txtFileName.Text = _fileInfo.FileName;
            txtCurrentVersion.Text = _fileInfo.Version;
            Version version = new Version(_fileInfo.Version);
            Version newVersion = version.IncrementMinor();
            txtNextVersion.Text = newVersion.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void EventListener(FileInfoDTO fileInfo)
        {
            _fileInfo = fileInfo;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
        }
    }
}
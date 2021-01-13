using MetroFramework.Forms;
using System;
using System.IO;

namespace ECMCS.App
{
    public partial class frmSyncToECM : MetroForm
    {
        public frmSyncToECM()
        {
            InitializeComponent();
            txtFileName.ReadOnly = true;
            txtOwner.ReadOnly = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void EventListener((string fileName, string owner) fileInfo)
        {
            txtFileName.Text = Path.GetFileName(fileInfo.fileName);
            txtOwner.Text = fileInfo.owner;
        }
    }
}
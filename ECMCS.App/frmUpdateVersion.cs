using ECMCS.DTO;
using ECMCS.Utilities;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace ECMCS.App
{
    public partial class frmUpdateVersion : MetroForm
    {
        private FileInfoDTO _fileInfo;
        private string uploadUrl = $"{ConfigHelper.Read("BaseUrl")}/filehistory/IncreaseVersion";

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
            try
            {
                UploadFile();
                _fileInfo.IsUploaded = true;
                JsonHelper.Update(_fileInfo, x => x.FilePath == _fileInfo.FilePath);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during processing{Environment.NewLine}{ex.Message}", "Warning");
            }
        }

        private void UploadFile()
        {
            var fileUpload = new FileUploadDTO();
            fileUpload.Id = _fileInfo.Id;
            fileUpload.FileName = _fileInfo.FileName;
            fileUpload.Modifier = "anhhuy.le";
            fileUpload.Version = rdUpdateNextVersion.Checked ? txtNextVersion.Text : _fileInfo.Version;
            fileUpload.FileData = File.ReadAllBytes(_fileInfo.FilePath);

            var json = JsonConvert.SerializeObject(fileUpload);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = client.PostAsync(uploadUrl, data).Result;
            _ = response.Content.ReadAsStringAsync().Result;
        }

        private void frmUpdateVersion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_fileInfo.IsUploaded)
            {
                DialogResult dialog = MessageBox.Show($"This document has not uploaded to ECM server.{Environment.NewLine}Close form?", "Warning", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void frmUpdateVersion_FormClosed(object sender, FormClosedEventArgs e)
        {
            UploadClosed(_fileInfo);
        }

        private event EventHandler<FileInfoDTO> onUploadClosed;

        public event EventHandler<FileInfoDTO> OnUploadClosed
        {
            add
            {
                onUploadClosed += value;
            }
            remove
            {
                onUploadClosed -= value;
            }
        }

        private void UploadClosed(FileInfoDTO fileInfo)
        {
            if (onUploadClosed != null)
            {
                onUploadClosed(this, fileInfo);
            }
        }
    }
}
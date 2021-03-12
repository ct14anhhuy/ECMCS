using ECMCS.DTO;
using ECMCS.Utilities;
using ECMCS.Utilities.FileFolderExtensions;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace ECMCS.App
{
    public partial class frmUpdateVersion : MetroForm
    {
        private FileDownloadDTO _fileInfo;
        private readonly JsonHelper _jsonHelper;

        public frmUpdateVersion()
        {
            InitializeComponent();
            _jsonHelper = new JsonHelper();
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
            Close();
        }

        public void EventListener(FileDownloadDTO fileInfo)
        {
            _fileInfo = fileInfo;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile();
                if (!_fileInfo.IsUploaded)
                {
                    throw new Exception();
                }
                _jsonHelper.Update(_fileInfo, x => x.FilePath == _fileInfo.FilePath);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during processing{Environment.NewLine}{ex.Message}", "Warning");
            }
        }

        private void UploadFile()
        {
            string uploadUrl = $"{SystemParams.API_URL}/FileHistory/UploadFile";
            var fileUpload = new FileUploadDTO();
            fileUpload.FileId = _fileInfo.Id;
            fileUpload.FileName = _fileInfo.FileName;
            fileUpload.ModifierUser = CheckModifier();
            fileUpload.Version = rdUpdateNextVersion.Checked ? txtNextVersion.Text : _fileInfo.Version;
            fileUpload.FileData = File.ReadAllBytes(_fileInfo.FilePath);
            fileUpload.Size = fileUpload.FileData.Length / 1024;

            var json = JsonConvert.SerializeObject(fileUpload);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new EcmHttpClient(_fileInfo.Owner);
            var response = client.PostAsync(uploadUrl, data).Result;
            _fileInfo.IsUploaded = response.IsSuccessStatusCode;
        }

        private string CheckModifier()
        {
            JsonHelper jsonHelper = new JsonHelper(CommonConstants.USER_FILE);
            var json = jsonHelper.Get<object>().FirstOrDefault();
            var emp = JsonConvert.DeserializeAnonymousType(json.ToString(), new { epLiteId = "" });
            return emp.epLiteId;
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

        private event EventHandler<FileDownloadDTO> onUploadClosed;

        public event EventHandler<FileDownloadDTO> OnUploadClosed
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

        private void UploadClosed(FileDownloadDTO fileInfo)
        {
            if (onUploadClosed != null)
            {
                onUploadClosed(this, fileInfo);
            }
        }
    }
}
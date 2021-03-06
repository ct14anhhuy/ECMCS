﻿using ECMCS.DTO;
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
            var fileUpload = new FileUploadDTO
            {
                FileId = _fileInfo.Id,
                FileName = _fileInfo.FileName,
                ModifierUser = GetModifier(),
                Version = rdUpdateNextVersion.Checked ? txtNextVersion.Text : _fileInfo.Version,
                FileData = File.ReadAllBytes(_fileInfo.FilePath)
            };
            var json = JsonConvert.SerializeObject(fileUpload);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new EcmHttpClient(epLiteId: _fileInfo.Owner);
            var response = client.PostAsync(uploadUrl, data).Result;
            _fileInfo.IsUploaded = response.IsSuccessStatusCode;
        }

        private string GetModifier()
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
            onUploadClosed?.Invoke(this, fileInfo);
        }
    }
}
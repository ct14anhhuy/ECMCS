﻿using ECMCS.DTO;
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
            UploadFile();
            _fileInfo.IsUploaded = true;
            this.Close();
        }

        private async void UploadFile()
        {
            var fileUpload = new FileUploadDTO();
            fileUpload.Id = _fileInfo.Id;
            fileUpload.Owner = _fileInfo.Owner;
            fileUpload.FileName = _fileInfo.FileName;
            fileUpload.Version = rdUpdateNextVersion.Checked ? txtNextVersion.Text : _fileInfo.Version;
            fileUpload.FileData = File.ReadAllBytes(_fileInfo.FilePath);

            var json = JsonConvert.SerializeObject(fileUpload);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "http://localhost:53115/api/file/upload";
            var client = new HttpClient();
            var response = await client.PostAsync(url, data);
            string result = response.Content.ReadAsStringAsync().Result;
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
            uploadClosed(_fileInfo);
        }

        private event EventHandler<FileInfoDTO> _onUploadClosed;

        public event EventHandler<FileInfoDTO> OnUploadClosed
        {
            add
            {
                _onUploadClosed += value;
            }
            remove
            {
                _onUploadClosed -= value;
            }
        }

        private void uploadClosed(FileInfoDTO fileInfo)
        {
            if (_onUploadClosed != null)
            {
                _onUploadClosed(this, fileInfo);
            }
        }
    }
}
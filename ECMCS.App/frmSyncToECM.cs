﻿using ECMCS.DTO;
using ECMCS.Utilities;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace ECMCS.App
{
    public partial class frmSyncToECM : MetroForm
    {
        private int _curId = 0;
        private string _fullPath;
        private bool _uploadStatus = false;

        public frmSyncToECM()
        {
            InitializeComponent();
            txtFileName.ReadOnly = true;
            txtOwner.ReadOnly = true;
            txtPath.ReadOnly = true;
        }

        private void frmSyncToECM_Load(object sender, EventArgs e)
        {
            GetECMDirectory();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void EventListener((string fullPath, string owner) fileInfo)
        {
            txtFileName.Text = StringHelper.RemoveSharpCharacter(Path.GetFileName(fileInfo.fullPath));
            txtOwner.Text = fileInfo.owner;
            _fullPath = fileInfo.fullPath;
        }

        private async void GetECMDirectory()
        {
            string url = $"{SystemParams.API_URL}/directory/GetTreeDirectory";
            var client = new EcmHttpClient(txtOwner.Text);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string resData = await response.Content.ReadAsStringAsync();
                List<DirectoryDTO> directories = JsonConvert.DeserializeObject<List<DirectoryDTO>>(resData);
                InitTreeView(directories);
            }
        }

        private void InitTreeView(List<DirectoryDTO> directories)
        {
            var dict = new Dictionary<int, TreeNode>();
            var orphans = new Queue<DirectoryDTO>();
            foreach (DirectoryDTO item in directories)
            {
                TreeNode newNode = new TreeNode(item.Name);
                if (dict.TryGetValue(Convert.ToInt32(item.ParentId), out TreeNode parent))
                {
                    parent.Nodes.Add(newNode);
                }
                else if (item.ParentId == 0)
                {
                    treeECMFolder.Nodes.Add(newNode);
                }
                else
                {
                    orphans.Enqueue(item);
                }
                dict.Add(item.Id, newNode);
                newNode.Tag = item.Id;
            }
            foreach (DirectoryDTO item in orphans)
            {
                TreeNode orphan = dict[item.Id];
                if (dict.TryGetValue(Convert.ToInt32(item.ParentId), out TreeNode parent))
                {
                    parent.Nodes.Add(orphan);
                }
                else
                {
                    treeECMFolder.Nodes.Add(orphan);
                }
                orphan.Tag = item.Id;
            }
            SetIconForNode();
        }

        private void SetIconForNode()
        {
            int imgIndex = 0;
            ImageList list = new ImageList();
            Image img = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\folder.ico");
            list.Images.Add(img);
            treeECMFolder.ImageList = list;
            foreach (TreeNode node in treeECMFolder.Nodes)
            {
                SetIcon(node, imgIndex);
            }
        }

        private void SetIcon(TreeNode node, int imgIndex)
        {
            node.ImageIndex = imgIndex;
            node.SelectedImageIndex = imgIndex;
            if (node.Nodes.Count != 0)
            {
                foreach (TreeNode tn in node.Nodes)
                {
                    SetIcon(tn, imgIndex);
                }
            }
        }

        private void treeECMFolder_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtPath.Text = e.Node.FullPath;
            _curId = (int)e.Node.Tag;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile();
                if (!_uploadStatus)
                {
                    throw new Exception();
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during processing{Environment.NewLine}{ex.Message}", "Warning");
            }
        }

        private void UploadFile()
        {
            string uploadUrl = $"{SystemParams.API_URL}/FileInfo/UploadNewFile";
            FileInfoDTO fileInfo = new FileInfoDTO
            {
                Name = txtFileName.Text,
                OwnerUser = txtOwner.Text,
                DirectoryId = _curId,
                Tag = txtTag.Text,
                FileData = File.ReadAllBytes(_fullPath)
            };
            var json = JsonConvert.SerializeObject(fileInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new EcmHttpClient(fileInfo.OwnerUser);
            var response = client.PostAsync(uploadUrl, data).Result;
            _uploadStatus = response.IsSuccessStatusCode;
        }

        private event EventHandler<bool> onUploadClosed;

        public event EventHandler<bool> OnUploadClosed
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

        private void UploadClosed(bool uploadStatus)
        {
            onUploadClosed?.Invoke(this, uploadStatus);
        }

        private void frmSyncToECM_FormClosed(object sender, FormClosedEventArgs e)
        {
            UploadClosed(_uploadStatus);
        }
    }
}
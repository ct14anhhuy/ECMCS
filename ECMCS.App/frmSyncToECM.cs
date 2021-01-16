using ECMCS.DTO;
using ECMCS.Utilities.FileFolderExtensions;
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
            this.Close();
        }

        public void EventListener((string fullPath, string owner) fileInfo)
        {
            txtFileName.Text = Path.GetFileName(fileInfo.fullPath);
            txtOwner.Text = fileInfo.owner;
            _fullPath = fileInfo.fullPath;
        }

        private async void GetECMDirectory()
        {
            string url = $"{ConfigHelper.Read("ApiUrl")}/directory";
            HttpClient client = new HttpClient();
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
                TreeNode parent;
                if (dict.TryGetValue(Convert.ToInt32(item.ParentId), out parent))
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
                TreeNode parent;
                if (dict.TryGetValue(Convert.ToInt32(item.ParentId), out parent))
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
                _uploadStatus = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during processing{Environment.NewLine}{ex.Message}", "Warning");
            }
        }

        private void UploadFile()
        {
            string uploadUrl = $"{ConfigHelper.Read("ApiUrl")}/fileinfo/uploadnewfile";
            FileInfoDTO fileInfo = new FileInfoDTO();
            fileInfo.Name = txtFileName.Text;
            fileInfo.OwnerUser = txtOwner.Text;
            fileInfo.DirectoryId = _curId;
            fileInfo.Tag = txtTag.Text;
            fileInfo.FileData = File.ReadAllBytes(_fullPath);

            var json = JsonConvert.SerializeObject(fileInfo);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = new HttpClient();
            var response = client.PostAsync(uploadUrl, data).Result;
            _ = response.Content.ReadAsStringAsync().Result;
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
            if (onUploadClosed != null)
            {
                onUploadClosed(this, uploadStatus);
            }
        }

        private void frmSyncToECM_FormClosed(object sender, FormClosedEventArgs e)
        {
            UploadClosed(_uploadStatus);
        }
    }
}
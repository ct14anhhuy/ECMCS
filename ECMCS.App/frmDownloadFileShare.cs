using ECMCS.App.Models;
using ECMCS.Utilities;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;

namespace ECMCS.App
{
    public partial class frmDownloadFileShare : MetroForm
    {
        private EcmHttpClient _httpClient;
        private string[] _fileUrls;

        public frmDownloadFileShare()
        {
            InitializeComponent();
        }

        public void EventListener((string epLiteId, int fileId) data)
        {
            _httpClient = new EcmHttpClient(data.epLiteId);
            GetFileInfo(data.fileId);
            GetFileShareUrl(data.fileId);
        }

        private async void GetFileShareUrl(int fileId)
        {
            _fileUrls = new string[3];
            string url = $"{SystemParams.API_URL}/FileInfo/GetFileUrl?id={fileId}&isShareUrl=true";
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resData = await response.Content.ReadAsStringAsync();
                _fileUrls = JsonConvert.DeserializeObject<string[]>(resData);
                _fileUrls[0] = "ECMProtocol: " + _fileUrls[0];
                _fileUrls[1] = "ECMProtocol: " + _fileUrls[1];
            }
        }

        private async void GetFileInfo(int fileId)
        {
            string url = $"{SystemParams.API_URL}/FileInfo/GetFileInfo?id=" + fileId;
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var resData = await response.Content.ReadAsStringAsync();
                var fileInfo = JsonConvert.DeserializeObject<FileInfoViewModel>(resData);
                txtCreatedDate.Text = fileInfo.CreatedDate.ToString();
                txtFileName.Text = fileInfo.Name;
                txtVersion.Text = fileInfo.Version.ToString();
                txtCreatedDate.Text = Convert.ToDateTime(fileInfo.CreatedDate).ToShortDateString();
                txtModifiedDate.Text = Convert.ToDateTime(fileInfo.ModifiedDate).ToShortDateString();
                txtOwner.Text = fileInfo.Owner.ToString();
                txtSecurityLevel.Text = fileInfo.SecurityLevel;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            OpenURL(_fileUrls[0]);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            OpenURL(_fileUrls[1]);
        }

        private void OpenURL(string url)
        {
            WebBrowser webBrowser = new WebBrowser();
            try
            {
                webBrowser.Navigate(new Uri(url));
                Close();
            }
            catch (UriFormatException ex)
            {
                throw ex;
            }
        }
    }
}
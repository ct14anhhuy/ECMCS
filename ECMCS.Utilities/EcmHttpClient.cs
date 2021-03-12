using ECMCS.Utilities.FileFolderExtensions;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ECMCS.Utilities
{
    public class EcmHttpClient : HttpClient
    {
        private string _epLiteId;

        public EcmHttpClient(string epLiteId)
        {
            _epLiteId = epLiteId;
            string getTokenUrl = $"{SystemParams.API_URL}/Token/GetToken?epLiteId=" + _epLiteId;
            var client = new HttpClient();
            var response = client.GetAsync(getTokenUrl).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string accessToken = Regex.Replace(result, "\\\"", "");
            DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        public string GetToken()
        {
            string getTokenUrl = $"{SystemParams.API_URL}/Token/GetToken?epLiteId=" + _epLiteId;
            var client = new HttpClient();
            var response = client.GetAsync(getTokenUrl).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string accessToken = Regex.Replace(result, "\\\"", "");
            return accessToken;
        }
    }
}
using ECMCS.Utilities.FileFolderExtensions;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ECMCS.App.Extension
{
    public class EcmHttpClient : HttpClient
    {
        public EcmHttpClient(string epLiteId)
        {
            string uploadUrl = $"{ConfigHelper.Read("ApiUrl")}/Token/GetToken?epLiteId=" + epLiteId;
            var client = new HttpClient();
            var response = client.GetAsync(uploadUrl).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            string accessToken = Regex.Replace(result, "\\\"", "");
            DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }
    }
}
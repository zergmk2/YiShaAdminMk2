using System;
using System.Threading.Tasks;
using YiSha.Util.Helper;
using RestSharp;

namespace YiSha.Util
{
    public class RestClientHelper<T>
    {
        public static async Task<T> SendRestRequestAsync(string address, string port, RestRequest req)
        {
            var uriBuilder = new UriBuilder();
            uriBuilder.Host = address;
            uriBuilder.Port = port.ParseToInt();
            LogHelper.Debug("Set url to :" + uriBuilder.ToString());
            var restClient = new RestClient(uriBuilder.Uri);
            var resp = await restClient.ExecuteAsync<T>(req);
            return resp.Data;
        }
    }
}

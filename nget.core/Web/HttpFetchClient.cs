using System.Net;

namespace nget.core.Web
{
    public class HttpFetchClient : IFetchClient
    {
        public void DownloadUrlToFile(string url, string targetFile)
        {
            var webClient = new WebClient();
            webClient.DownloadFile(url,targetFile);
        }
    }
}
using System.Net;

namespace nget.core.Web
{
    public class NWebClient : INWebClient
    {
        public void DownloadUrlToFile(string url, string targetFile)
        {
            var webClient = new WebClient();
            webClient.DownloadFile(url,targetFile);
        }
    }
}
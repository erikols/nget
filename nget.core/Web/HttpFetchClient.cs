using System;
using System.Net;

namespace nget.core.Web
{
    public class HttpFetchClient : IFetchClient
    {
        const string DefaultUserAgent = "nget/0.1";

        public void DownloadUrlToFile(string url, string targetFile)
        {
            var webClient = SetupClient();
            webClient.DownloadFile(url, targetFile);
            Console.Write("\n");
        }

        static WebClient SetupClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", DefaultUserAgent);
            webClient.DownloadProgressChanged += OnDownloadProgressChanged;
            return webClient;
        }

        static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.Write('.');
        }
    }
}
using System;
using System.Net;

namespace nget.core.Web
{
    public class HttpFetchClient : IFetchClient
    {
        readonly IHttpRetryService httpRetryService;
        const string DefaultUserAgent = "nget/0.1";

        public HttpFetchClient(IHttpRetryService httpRetryService)
        {
            this.httpRetryService = httpRetryService;
        }

        public void DownloadUrlToFile(string url, string targetFile)
        {
            var webClient = SetupClient();

            httpRetryService.WithRetry(() => webClient.DownloadFile(url, targetFile));
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
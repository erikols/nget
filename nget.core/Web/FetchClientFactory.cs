using System;
using nget.core.S3;

namespace nget.core.Web
{
    public class FetchClientFactory : IFetchClientFactory
    {
        public IFetchClient GetDownloaderForUrl(string url)
        {
            var u = new Uri(url);
            if (u.Scheme == "http" || u.Scheme == "https")
                return Container.GetService<HttpFetchClient>();

            if (u.Scheme == "s3")
                return Container.GetService<S3FetchClient>();

            throw new ArgumentException("Unsupported protocol scheme: " + u.Scheme);
        }
    }
}
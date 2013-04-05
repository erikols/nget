using System;

namespace nget.core.S3
{
    public class S3Url
    {
        readonly string url;
        string bucket;
        string key;

        public S3Url(string url)
        {
            this.url = url;
            ParseUrl();
        }

        void ParseUrl()
        {
            var u = new Uri(url);
            if (u.Scheme != "s3")
                throw new ArgumentException("S3Url only supports the s3 protocol scheme");

            bucket = u.Host;
            key = u.PathAndQuery;
            if (key.StartsWith("/"))
                key = key.Substring(1);
        }

        public string Bucket
        {
            get { return bucket; }
        }

        public string Key
        {
            get { return key; }
        }
    }
}
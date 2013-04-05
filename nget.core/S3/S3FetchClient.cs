using System;
using Amazon.S3;
using Amazon.S3.Model;
using nget.core.Fs;
using nget.core.Web;

namespace nget.core.S3
{
    // http://docs.aws.amazon.com/sdkfornet/latest/apidocs/Index.html
    public class S3FetchClient : IFetchClient
    {
        readonly IHttpRetryService httpRetryService;
        readonly IFileSystem fileSystem;
        readonly AmazonS3 s3;

        public S3FetchClient(IHttpRetryService httpRetryService,
                             IFileSystem fileSystem,
                             AmazonS3 s3)
        {
            this.httpRetryService = httpRetryService;
            this.fileSystem = fileSystem;
            this.s3 = s3;
        }

        public void DownloadUrlToFile(string url, string targetFile)
        {
            if (url == null) throw new ArgumentNullException("url");
            if (targetFile == null) throw new ArgumentNullException("targetFile");

            var request = SetupGetRequest(new S3Url(url));
            PerformGetRequest(targetFile, request);
        }

        static GetObjectRequest SetupGetRequest(S3Url s3Url)
        {
            return new GetObjectRequest
                {
                    BucketName = s3Url.Bucket,
                    Key = s3Url.Key
                };
        }

        void PerformGetRequest(string targetFile, GetObjectRequest request)
        {
            using (var response = httpRetryService.WithRetry(() => s3.GetObject(request)))
            {
                using (var stream = response.ResponseStream)
                {
                    using (var outputStream = fileSystem.CreateWriteStream(targetFile))
                    {
                        stream.CopyTo(outputStream);
                    }
                }
            }
        }
    }
}
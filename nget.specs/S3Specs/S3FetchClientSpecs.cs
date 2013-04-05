using System;
using Amazon.S3;
using Amazon.S3.Model;
using Machine.Specifications;
using Moq;
using PS.Utilities.Specs;
using nget.core.Fs;
using nget.core.S3;
using nget.core.Web;
using nget.specs.TestDoubles;
using It = Machine.Specifications.It;
using Param = Moq.It;

namespace nget.specs.S3Specs
{
    [Subject(typeof (S3FetchClient))]
    public class S3FetchClientSpecs : With_an_automocked<S3FetchClient>
    {
        Establish context = () =>
            {
                url = "s3://bucket/path/key.ext";
                targetFile = "some-file.ext";

                mockS3 = GetTestDouble<AmazonS3>();
                mockS3.Setup(x => x.GetObject(Param.IsAny<GetObjectRequest>())).
                       Returns(new GetObjectResponse
                           {
                               ResponseStream = new FakeStream()
                           });

                // call whatever func we get, but use the mock to record it
                mockRetryService = GetTestDouble<IHttpRetryService>();
                mockRetryService.Setup(
                    x => x.WithRetry(Param.IsAny<Func<GetObjectResponse>>()))
                                .Returns((Func<GetObjectResponse> func) => func());

                mockFileSystem = GetTestDouble<IFileSystem>();
                mockFileSystem.Setup(x => x.CreateWriteStream(Param.IsAny<string>()))
                    .Returns(new FakeStream());
            };

        Because of = () => ClassUnderTest.DownloadUrlToFile(url, targetFile);

        It should_call_the_s3_client_with_the_expected_params
            = () => mockS3
                        .Verify(x => x.GetObject(Param.Is<GetObjectRequest>(y =>
                                                                            y.BucketName == "bucket" &&
                                                                            y.Key == "path/key.ext"
                                                     )));

        static string url;
        static string targetFile;
        static Mock<IHttpRetryService> mockRetryService;
        static Mock<AmazonS3> mockS3;
        static Mock<IFileSystem> mockFileSystem;
    }
}
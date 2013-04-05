using System;
using Machine.Specifications;
using nget.core.S3;

namespace nget.specs.S3Specs
{
    [Subject(typeof (S3Url))]
    public class When_parsing_an_s3_url
    {
        Establish context = () => { url = "s3://bucket-name/path/to/file.ext"; };

        Because of = () => s3Url = new S3Url(url);

        It should_parse_and_return_the_bucket = () => s3Url.Bucket.ShouldEqual("bucket-name");
        It should_parse_and_return_the_key_as_the_rest_of_the_url = () => s3Url.Key.ShouldEqual("path/to/file.ext");

        static S3Url s3Url;
        static string url;
    }

    [Subject(typeof (S3Url))]
    public class When_parsing_a_non_s3_url
    {
        Establish context = () => { url = "http://host/path/to/file.ext"; };

        Because of = () => exception = Catch.Exception(() => s3Url = new S3Url(url));

        It should_throw_an_ArgumentException = () => exception.ShouldBeOfType<ArgumentException>();

        static S3Url s3Url;
        static string url;
        static Exception exception;
    }
}
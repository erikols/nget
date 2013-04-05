using Machine.Specifications;
using PS.Utilities.Specs;
using nget.core.Utils;

namespace nget.specs.UtilSpecs
{
    [Subject(typeof (UrlExpander))]
    public class When_expanding_a_fully_qualified_HTTP_url : With_an_automocked<UrlExpander>
    {
        Because of = () => expandedUrl = ClassUnderTest.ExpandUrl(url);

        It should_return_the_URL_unmodified = () => expandedUrl.ShouldEqual(url);

        static string expandedUrl;
        const string url = "http://host/path/file.ext";
    }

    [Subject(typeof(UrlExpander))]
    public class When_expanding_a_fully_qualified_S3_url : With_an_automocked<UrlExpander>
    {
        Because of = () => expandedUrl = ClassUnderTest.ExpandUrl(url);

        It should_return_the_URL_unmodified = () => expandedUrl.ShouldEqual(url);

        static string expandedUrl;
        const string url = "s3://bucket/path/file.ext";
    }

    [Subject(typeof(UrlExpander))]
    public class When_expanding_a_raw_host_name : With_an_automocked<UrlExpander>
    {
        Because of = () => expandedUrl = ClassUnderTest.ExpandUrl(url);

        It should_return_a_prepended_http_protocol_scheme = () => expandedUrl.ShouldEqual("http://" + url);

        static string expandedUrl;
        const string url = "hostname";
    }
    
    [Subject(typeof(UrlExpander))]
    public class When_expanding_a_host_and_path_without_a_scheme : With_an_automocked<UrlExpander>
    {
        Because of = () => expandedUrl = ClassUnderTest.ExpandUrl(url);

        It should_return_a_prepended_http_protocol_scheme = () => expandedUrl.ShouldEqual("http://" + url);

        static string expandedUrl;
        const string url = "hostname/path/file.ext";
    }
}
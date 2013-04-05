using Machine.Specifications;
using PS.Utilities.Specs;
using nget.core.FileName;

namespace nget.specs.FileNameSpecs
{
    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_an_URL : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl("http://somesite.com/vdir/some_file.ext");

        It should_extract_the_last_path_component_as_the_filename = () => fileName.ShouldEqual("some_file.ext");

        static string fileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_an_URL_with_only_one_slash : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl("http://somesite.com/some_file.ext");

        It should_extract_the_last_path_component_as_the_filename = () => fileName.ShouldEqual("some_file.ext");

        static string fileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_an_URL_without_an_extension : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl("http://somesite.com/some_file");

        It should_extract_the_last_path_component_as_the_filename = () => fileName.ShouldEqual("some_file");

        static string fileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_an_URL_with_only_a_host : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl("http://somesite.com");

        It should_use_the_hostname_sans_dots_as_the_filename = () => fileName.ShouldEqual("somesite_com");

        static string fileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_an_empty_string : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl("");

        It should_return_an_empty_filename = () => fileName.ShouldBeEmpty();

        static string fileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_deriving_a_filename_from_a_null_string : With_an_automocked<FileNameDeriver>
    {
        Because of = () => fileName = ClassUnderTest.FilenameFromUrl(null);

        It should_return_an_empty_filename = () => fileName.ShouldBeEmpty();

        static string fileName;
    }
}
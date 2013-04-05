using Machine.Specifications;
using PS.Utilities.Specs;
using nget.core.FileName;

namespace nget.specs.FileNameSpecs
{
    [Subject(typeof(FileNameDeriver))]
    public class When_incrementing_a_filename_without_a_version : With_an_automocked<FileNameDeriver>
    {
        Establish context = () => { fileName = "some_file.ext"; };

        Because of = () => newFileName = ClassUnderTest.IncrementFileName(fileName);

        It should_append_a_one_to_the_filename_portion = () => newFileName.ShouldEqual("some_file(1).ext");
        
        static string fileName;
        static string newFileName;
    }

    [Subject(typeof(FileNameDeriver))]
    public class When_incrementing_a_filename_with_a_path_without_a_version : With_an_automocked<FileNameDeriver>
    {
        Establish context = () => { fileName = @"c:\dir1\dir2\some_file.ext"; };

        Because of = () => newFileName = ClassUnderTest.IncrementFileName(fileName);

        It should_append_a_one_to_the_filename_portion = () => newFileName.ShouldEqual(@"c:\dir1\dir2\some_file(1).ext");

        static string fileName;
        static string newFileName;
    }

    [Subject(typeof(FileNameDeriver))]
    public class When_incrementing_a_filename_with_a_version : With_an_automocked<FileNameDeriver>
    {
        Establish context = () => { fileName = "some_file(2).ext"; };

        Because of = () => newFileName = ClassUnderTest.IncrementFileName(fileName);

        It should_increment_the_version = () => newFileName.ShouldEqual("some_file(3).ext");

        static string fileName;
        static string newFileName;
    }

    [Subject(typeof(FileNameDeriver))]
    public class When_incrementing_a_filename_with_a_path_and_a_version : With_an_automocked<FileNameDeriver>
    {
        Establish context = () => { fileName = @"c:\dir1\dir2\some_file(2).ext"; };

        Because of = () => newFileName = ClassUnderTest.IncrementFileName(fileName);

        It should_increment_the_version = () => newFileName.ShouldEqual(@"c:\dir1\dir2\some_file(3).ext");

        static string fileName;
        static string newFileName;
    }

    [Subject(typeof(FileNameDeriver))]
    public class When_incrementing_a_filename_with_a_double_digit_version : With_an_automocked<FileNameDeriver>
    {
        Establish context = () => { fileName = "some_file(19).ext"; };

        Because of = () => newFileName = ClassUnderTest.IncrementFileName(fileName);

        It should_increment_the_version = () => newFileName.ShouldEqual("some_file(20).ext");

        static string fileName;
        static string newFileName;
    }
}
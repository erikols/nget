using System.IO;
using Machine.Specifications;
using PS.Utilities.Specs;
using nget.core.FileName;

namespace nget.specs.FileNameSpecs
{
    [Subject(typeof (FileNameDeriver))]
    public class When_generating_a_temporary_file_in_the_current_directory : With_an_automocked<FileNameDeriver>
    {
        Because of = () => tempFileName = ClassUnderTest.GetTempFileForTarget(fileName);

        It should_return_a_temp_file_without_a_path = () => Path.GetDirectoryName(tempFileName).ShouldBeEmpty();

        const string fileName = "some_file.ext";
        static string tempFileName;
    }

    [Subject(typeof (FileNameDeriver))]
    public class When_generating_a_temporary_file_in_an_explicit_directory : With_an_automocked<FileNameDeriver>
    {
        Because of = () => tempFileName = ClassUnderTest.GetTempFileForTarget(fileName);

        It should_return_a_temp_file_in_the_target_directory = () => Path.GetDirectoryName(tempFileName).ShouldEqual(@"c:\dir1\dir2");

        const string fileName = @"c:\dir1\dir2\some_file.ext";
        static string tempFileName;
    }
}
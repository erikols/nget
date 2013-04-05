using System;
using Machine.Specifications;
using Moq;
using PS.Utilities.Specs;
using nget.core.FileName;
using nget.core.Fs;
using nget.core.Web;
using It = Machine.Specifications.It;

namespace nget.specs.DownloaderSpecs
{
    [Subject(typeof (FileDownloader))]
    public class When_downloading_a_file : With_an_automocked<FileDownloader>
    {
        Establish context = () =>
            {
                url = "http://host/path/file_name.ext";
                fileName = "file_name.ext";
                tempFileName = "abc123";

                mockFilenameDeriver = GetTestDouble<IFileNameDeriver>();
                mockFilenameDeriver.Setup(x => x.FilenameFromUrl(url)).Returns(fileName);
                mockFilenameDeriver.Setup(x => x.GetTempFileForTarget(fileName)).Returns(tempFileName);

                mockWebClient = GetTestDouble<IFetchClient>();
                mockFileSystem = GetTestDouble<IFileSystem>();
                GetTestDouble<IFetchClientFactory>()
                    .Setup(x => x.GetDownloaderForUrl(url))
                    .Returns(mockWebClient.Object);
            };

        Because of = () => ClassUnderTest.DownloadFile(url);

        It should_derive_the_filename = () => mockFilenameDeriver.Verify(x => x.FilenameFromUrl(url));
        It should_derive_a_random_temp_file = () => mockFilenameDeriver.Verify(x => x.GetTempFileForTarget(fileName));
        It should_call_the_web_client_with_a_tempfile = () => mockWebClient.Verify(x => x.DownloadUrlToFile(url, tempFileName));
        It should_rename_the_file_after_the_download_is_complete = () => mockFileSystem.Verify(x => x.RenameFile(tempFileName, fileName));

        static string url;
        static Mock<IFileNameDeriver> mockFilenameDeriver;
        static string fileName;
        static Mock<IFetchClient> mockWebClient;
        static string tempFileName;
        static Mock<IFileSystem> mockFileSystem;
    }

    [Subject(typeof (FileDownloader))]
    public class When_downloading_a_file_with_an_exception : With_an_automocked<FileDownloader>
    {
        Establish context = () =>
            {
                url = "http://host/path/file_name.ext";
                fileName = "file_name.ext";
                tempFileName = "abc123";

                mockFilenameDeriver = GetTestDouble<IFileNameDeriver>();
                mockFilenameDeriver.Setup(x => x.FilenameFromUrl(url)).Returns(fileName);
                mockFilenameDeriver.Setup(x => x.GetTempFileForTarget(fileName)).Returns(tempFileName);

                mockWebClient = GetTestDouble<IFetchClient>();
                mockFileSystem = GetTestDouble<IFileSystem>();
                GetTestDouble<IFetchClientFactory>()
                    .Setup(x => x.GetDownloaderForUrl(url))
                    .Returns(mockWebClient.Object);

                mockWebClient
                    .Setup(x => x.DownloadUrlToFile(Moq.It.IsAny<string>(), Moq.It.IsAny<string>()))
                    .Throws(new Exception("some error"));
            };

        Because of = () => exception = Catch.Exception(() => ClassUnderTest.DownloadFile(url));

        It should_derive_the_filename = () => mockFilenameDeriver.Verify(x => x.FilenameFromUrl(url));
        It should_derive_a_random_temp_file = () => mockFilenameDeriver.Verify(x => x.GetTempFileForTarget(fileName));
        It should_call_the_web_client_with_a_tempfile = () => mockWebClient.Verify(x => x.DownloadUrlToFile(url, tempFileName));
        It should_NOT_rename_the_file_after_the_download_is_complete = () => mockFileSystem.Verify(x => x.RenameFile(tempFileName, fileName), Times.Never());
        It should_instead_delete_the_temp_file = () => mockFileSystem.Verify(x => x.DeleteFile(tempFileName));
        It should_rethrow_the_exception = () => exception.ShouldNotBeNull();

        static string url;
        static Mock<IFileNameDeriver> mockFilenameDeriver;
        static string fileName;
        static Mock<IFetchClient> mockWebClient;
        static string tempFileName;
        static Mock<IFileSystem> mockFileSystem;
        static Exception exception;
    }
}
using System;
using nget.core.FileName;
using nget.core.Fs;

namespace nget.core.Web
{
    public class FileDownloader : IFileDownloader
    {
        readonly IFileNameDeriver fileNameDeriver;
        readonly IFetchClientFactory fetchClientFactory;
        readonly IFileSystem fileSystem;

        public FileDownloader(IFileNameDeriver fileNameDeriver,
                              IFetchClientFactory fetchClientFactory,
                              IFileSystem fileSystem)
        {
            this.fileNameDeriver = fileNameDeriver;
            this.fetchClientFactory = fetchClientFactory;
            this.fileSystem = fileSystem;
        }

        public void DownloadFile(string url)
        {
            if (url == null) throw new ArgumentNullException("url");

            var targetFile = fileNameDeriver.FilenameFromUrl(url);
            var temporaryDownloadFile = fileNameDeriver.GetTempFileForTarget(targetFile);
            try
            {
                var webClient = fetchClientFactory.GetDownloaderForUrl(url);
                webClient.DownloadUrlToFile(url, temporaryDownloadFile);
            }
            catch (Exception /* exception */)
            {
                fileSystem.DeleteFile(temporaryDownloadFile);
                throw;
            }
            fileSystem.RenameFile(temporaryDownloadFile, targetFile);
        }
    }
}
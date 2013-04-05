namespace nget.core.Web
{
    public interface IFetchClient
    {
        void DownloadUrlToFile(string url, string targetFile);
    }
}
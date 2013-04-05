namespace nget.core.Web
{
    public interface INWebClient
    {
        void DownloadUrlToFile(string url, string targetFile);
    }
}
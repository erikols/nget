namespace nget.core.Web
{
    public interface IFetchClientFactory
    {
        IFetchClient GetDownloaderForUrl(string url);
    }
}
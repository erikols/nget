using nget.core;
using nget.core.Web;

namespace nget
{
    internal class NGetMain
    {
        static void Main(string[] args)
        {
            Container.Init();
            foreach (var arg in args)
                ProcessUrl(arg);
        }

        static void ProcessUrl(string url)
        {
            var downloader = Container.GetService<IFileDownloader>();
            downloader.DownloadFile(url);
        }
    }
}
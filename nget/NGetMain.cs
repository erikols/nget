using System;
using nget.core;
using nget.core.Utils;
using nget.core.Web;

namespace nget
{
    internal class NGetMain
    {
        static void Main(string[] args)
        {
            try
            {
                Container.Init();

                if (args.Length == 3)
                {
                    if (args[0] == "--setCreds")
                    {
                        CreateS3CredFile(accessKey: args[1], secretKey: args[2]);
                        return;
                    }
                }

                foreach (var arg in args)
                    ProcessUrl(arg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured: {0}", ex);
            }
        }

        static void CreateS3CredFile(string accessKey, string secretKey)
        {
            var credentialService = Container.GetService<ICredentialService>();
            credentialService.ProtectAndPersistCredentials(credentialService.LocateCredentialFile(),
                                                           new ProtectedAwsCredentials
                                                               {
                                                                   AwsAccessKey = accessKey,
                                                                   AwsSecretKey = secretKey
                                                               });
        }

        static void ProcessUrl(string url)
        {
            var downloader = Container.GetService<IFileDownloader>();
            downloader.DownloadFile(url);
        }
    }
}
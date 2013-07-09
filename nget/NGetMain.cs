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
                if (args.Length == 0)
                {
                    ShowUsage();
                    return;
                }

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

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("nget url # download URL to local file");
            Console.WriteLine("\nS3 Credentials");
            Console.WriteLine("nget --setCreds s3-access-key s3-secret-key # store credentials");
            Console.WriteLine("nget --setCreds s3-access-key s3-secret-key # store credentials");
            Console.WriteLine("Supported schemes: http/https/s3");
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
            var urlExpander = Container.GetService<UrlExpander>();
            var downloader = Container.GetService<IFileDownloader>();
            downloader.DownloadFile(urlExpander.ExpandUrl(url));
        }
    }
}
using Amazon.Runtime;
using Amazon.S3;
using nget.core.Utils;

namespace nget.core.S3
{
    public class S3ClientFactory
    {
        readonly ICredentialService credentialService;

        public S3ClientFactory(ICredentialService credentialService)
        {
            this.credentialService = credentialService;
        }

        public AmazonS3 CreateClient()
        {
            var creds = credentialService.ReadAndDecryptCredentials();
            return new AmazonS3Client(new BasicAWSCredentials(creds.AwsAccessKey, creds.AwsSecretKey));
        }
    }
}
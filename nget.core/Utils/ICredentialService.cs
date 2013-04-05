namespace nget.core.Utils
{
    public interface ICredentialService
    {
        void ProtectAndPersistCredentials(string path, ProtectedAwsCredentials credentials);
        ProtectedAwsCredentials ReadAndDecryptCredentials();
        string LocateCredentialFile();
    }
}
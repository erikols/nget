#region Using directives

using System;
using Newtonsoft.Json;
using nget.core.Fs;

#endregion

namespace nget.core.Utils
{
    public class CredentialService : ICredentialService
    {
        readonly IFileSystem fileSystem;
        readonly IDataProtectionService dataProtectionService;

        public CredentialService(IFileSystem fileSystem, IDataProtectionService dataProtectionService)
        {
            this.fileSystem = fileSystem;
            this.dataProtectionService = dataProtectionService;
        }

        public void ProtectAndPersistCredentials(string path, ProtectedAwsCredentials credentials)
        {
            var json = JsonConvert.SerializeObject(credentials);
            var encryptedData = dataProtectionService.ProtectString(json);

            using (var fileStream = fileSystem.CreateWriteStream(path))
            {
                fileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        public ProtectedAwsCredentials ReadAndDecryptCredentials()
        {
            var path = LocateCredentialFile();

            if (!fileSystem.FileExists(path))
                throw new InvalidOperationException("Can't locate credential file at: " + path);

            var encryptedBytes = new byte[fileSystem.FileSize(path)];
            using (var fileStream = fileSystem.OpenReadStream(path))
            {
                fileStream.Read(encryptedBytes, 0, encryptedBytes.Length);
            }

            var clearTextJson = dataProtectionService.UnprotectString(encryptedBytes);
            return JsonConvert.DeserializeObject<ProtectedAwsCredentials>(clearTextJson);
        }

        public string LocateCredentialFile()
        {
            return ".creds";
        }
    }
}
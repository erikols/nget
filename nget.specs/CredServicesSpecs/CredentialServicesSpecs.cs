using System;
using Machine.Specifications;
using Moq;
using nget.core.Utils;
using It = Machine.Specifications.It;

namespace nget.specs.CredServicesSpecs
{
    [Subject(typeof(CredentialService))]
    public class When_roundtripping_credentials : With_a_FakeFileSystem
    {
        Establish context = () =>
                                {
                                    path = ".creds";
                                    var dpapiservice = new DataProtectionService();
                                    mockProtectionService = new Mock<IDataProtectionService>();
                                    Func<string, byte[]> protect = dpapiservice.ProtectString;
                                    Func<byte[], string> unprotect = dpapiservice.UnprotectString;

                                    mockProtectionService.Setup(x => x.ProtectString(Moq.It.IsAny<string>())).Returns(protect);
                                    mockProtectionService.Setup(x => x.UnprotectString(Moq.It.IsAny<byte[]>())).Returns(unprotect);

                                    credentialService = new CredentialService(fakeFileSystem, mockProtectionService.Object);
                                    credentials = new ProtectedAwsCredentials
                                                      {
                                                          AwsAccessKey = "access-key",
                                                          AwsSecretKey = "secret-key"
                                                      };

                                    credentialService.ProtectAndPersistCredentials(path, credentials);
                                };

        Because of = () => retrievedCredentials = credentialService.ReadAndDecryptCredentials();

        It Should_protect_the_data = () => mockProtectionService.Verify(x => x.ProtectString(Moq.It.IsAny<string>()), Times.Once());
        It Should_unprotect_the_data =
            () => mockProtectionService.Verify(x => x.UnprotectString(Moq.It.IsAny<byte[]>()), Times.Once());

        It Should_round_trip_the_AwsAccessKey_without_alteration =
            () => retrievedCredentials.AwsAccessKey.ShouldEqual(credentials.AwsAccessKey);
        It Should_round_trip_the_AwsSecretKey_without_alteration =
            () => retrievedCredentials.AwsSecretKey.ShouldEqual(credentials.AwsSecretKey);
        
        static CredentialService credentialService;
        static ProtectedAwsCredentials credentials;
        static ProtectedAwsCredentials retrievedCredentials;
        static string path;
        static Mock<IDataProtectionService> mockProtectionService;
    }
}
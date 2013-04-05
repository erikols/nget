using System.Security.Cryptography;
using System.Text;

namespace nget.core.Utils
{
    public class DataProtectionService : IDataProtectionService
    {
        public byte[] ProtectString(string clearText)
        {
            var clearTextBytes = Encoding.UTF8.GetBytes(clearText);
            return ProtectedData.Protect(clearTextBytes, null, DataProtectionScope.CurrentUser);
        }

        public string UnprotectString(byte[] cipherBytes)
        {
            var clearTextBytes = ProtectedData.Unprotect(cipherBytes, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(clearTextBytes);
        }
    }
}
namespace nget.core.Utils
{
    public interface IDataProtectionService
    {
        byte[] ProtectString(string clearText);
        string UnprotectString(byte[] cipherBytes);
    }
}
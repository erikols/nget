namespace nget.core.Fs
{
    public interface IFileSystem
    {
        void RenameFile(string sourceFileName, string destFileNAme);
        void DeleteFile(string fileToDelete);
    }
}
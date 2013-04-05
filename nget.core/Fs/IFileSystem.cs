using System.IO;

namespace nget.core.Fs
{
    public interface IFileSystem
    {
        void RenameFile(string sourceFileName, string destFileNAme);
        void DeleteFile(string fileToDelete);
        Stream CreateWriteStream(string path);
        bool FileExists(string path);
        long FileSize(string path);
        Stream OpenReadStream(string path);
    }
}
using System.IO;

namespace nget.core.Fs
{
    public class FileSystem : IFileSystem
    {
        public void RenameFile(string sourceFileName, string destFileNAme)
        {
            File.Move(sourceFileName, destFileNAme);
        }

        public void DeleteFile(string fileToDelete)
        {
            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
        }

        public Stream CreateWriteStream(string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public long FileSize(string path)
        {
            var fileInfo = new FileInfo(path);
            return fileInfo.Length;
        }

        public Stream OpenReadStream(string path)
        {
            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }
    }
}
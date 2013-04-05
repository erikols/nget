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
    }
}
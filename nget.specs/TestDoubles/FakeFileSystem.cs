#region using directives

using System.Collections.Generic;
using System.IO;
using nget.core.Fs;

#endregion

namespace nget.specs.TestDoubles
{
    public class FakeFileSystem : IFileSystem
    {
        readonly Dictionary<string, MemoryStream> inodes = new Dictionary<string, MemoryStream>();

        public void RenameFile(string sourceFileName, string destFileNAme)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFile(string fileToDelete)
        {
            throw new System.NotImplementedException();
        }

        public Stream CreateWriteStream(string path)
        {
            var contents = new MemoryStream();

            if (!inodes.ContainsKey(path))
                inodes.Add(path, contents);
            else
                inodes[path] = contents;
            return contents;
        }

        public Stream OpenReadStream(string path)
        {
            MemoryStream stream;
            inodes.TryGetValue(path, out stream);
            if (null != stream)
            {
                // ToArray works after disposing so this implementation needs to use that
                // instead of e.g. CopytoStream
                var clonedStream = new MemoryStream(stream.ToArray(), false);
                return clonedStream;
            }
            return null;
        }

        public string Slurp(string path)
        {
            var readStream = OpenReadStream(path);
            if (null == readStream)
                return null;

            using (var reader = new StreamReader(readStream))
            {
                return reader.ReadToEnd();
            }
        }

        public bool FileExists(string path)
        {
            return inodes.ContainsKey(path);
        }

        public long FileSize(string path)
        {
            MemoryStream file;

            // ToArray works after disposing so this implementation needs to use that
            // instead of e.g. Stream.Length
            if (inodes.TryGetValue(path, out file))
                return file.GetBuffer().LongLength;

            throw new FileNotFoundException("Test double unable to find inode key/path", path);
        }

        public bool DirectoryExists(string directory)
        {
            throw new System.NotImplementedException();
        }

        public void CreateDirectory(string directory)
        {
            throw new System.NotImplementedException();
        }
    }
}
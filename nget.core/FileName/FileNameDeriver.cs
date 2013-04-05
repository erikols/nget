using System.IO;
using System.Text.RegularExpressions;

namespace nget.core.FileName
{
    public class FileNameDeriver : IFileNameDeriver
    {
        static readonly Regex fileNameVersion = new Regex(@"\((\d+)\)\.");
        static readonly Regex fileNameVersionReplacement = new Regex(@"\(\d+\)\.");

        public string FilenameFromUrl(string url)
        {
            if (null == url)
                return string.Empty;

            var lastSlash = url.LastIndexOf('/');
            return lastSlash != -1
                       ? url.Substring(lastSlash + 1)
                       : string.Empty;
        }

        public string IncrementFileName(string fileName)
        {
            var match = fileNameVersion.Match(fileName);
            if (match.Success)
                return IncrementExistingVersion(fileName, match);

            return PatchInVersionNumber(fileName);
        }

        static string PatchInVersionNumber(string fileName)
        {
            var directoryPortion = Path.GetDirectoryName(fileName) ?? string.Empty;
            var baseName = Path.GetFileNameWithoutExtension(fileName);
            var ext = Path.GetExtension(fileName);
            return Path.Combine(directoryPortion, string.Format("{0}(1){1}", baseName, ext));
        }

        static string IncrementExistingVersion(string fileName, Match match)
        {
            var version = int.Parse(match.Groups[1].Value) + 1;
            var newVersion = string.Format("({0}).", version);
            return fileNameVersionReplacement.Replace(fileName, newVersion);
        }

        public string GetTempFileForTarget(string fileName)
        {
            var directoryPortion = Path.GetDirectoryName(fileName) ?? string.Empty;
            var randomName = Path.GetRandomFileName();
            return Path.Combine(directoryPortion, randomName);
        }
    }
}
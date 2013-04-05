namespace nget.core.FileName
{
    public interface IFileNameDeriver
    {
        string FilenameFromUrl(string url);
        string IncrementFileName(string fileName);
        string GetTempFileForTarget(string fileName);
    }
}
namespace StackLab
{
    public class FilePath
    {
        public string DirectoryPath { get; }
        public string Name { get; }
        public string FullPath => $"{DirectoryPath}/{Name}";

        public FilePath(string directoryPath, string fileName)
        {
            DirectoryPath = directoryPath;
            Name = fileName;
        }
    }
}

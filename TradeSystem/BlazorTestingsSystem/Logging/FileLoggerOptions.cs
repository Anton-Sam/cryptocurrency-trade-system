namespace BlazorTestingsSystem.Logging
{
    public class FileLoggerOptions
    {
        public string FolderPath { get; set; }
        public string FilePath { get; set; }
        public int MaxByteSize { get; set; }
    }
}

namespace ECMCS.DTO
{
    public class FileInfoDTO
    {
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
using System;

namespace ECMCS.DTO
{
    public class FileUploadDTO
    {
        public Guid FileId { get; set; }
        public string FileName { get; set; }
        public string ModifierUser { get; set; }
        public string Version { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public byte[] FileData { get; set; }
    }
}
using System;

namespace ECMCS.DTO
{
    public class FileInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Owner { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public string SecurityLevel { get; set; }
        public string Tag { get; set; }
        public int DirectoryId { get; set; }
        public byte[] FileData { get; set; }
        public string OwnerUser { get; set; }
    }
}
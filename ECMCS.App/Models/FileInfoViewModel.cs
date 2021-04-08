using System;

namespace ECMCS.App.Models
{
    public class FileInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Modifier { get; set; }
        public int Size { get; set; }
        public string SecurityLevel { get; set; }
        public string Version { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsImportant { get; set; }
    }
}
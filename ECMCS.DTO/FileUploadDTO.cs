﻿using System;

namespace ECMCS.DTO
{
    public class FileUploadDTO
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string ModifierUser { get; set; }
        public string Version { get; set; }
        public int Size { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public byte[] FileData { get; set; }
    }
}
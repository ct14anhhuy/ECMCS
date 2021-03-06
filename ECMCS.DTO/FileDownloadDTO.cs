﻿using System;

namespace ECMCS.DTO
{
    public class FileDownloadDTO
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Owner { get; set; }
        public string Modifier { get; set; }
        public string Version { get; set; }
        public bool ReadOnly { get; set; } = true;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public bool IsDone { get; set; } = false;
        public bool IsUploaded { get; set; } = false;
    }
}
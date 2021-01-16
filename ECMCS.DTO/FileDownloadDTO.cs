﻿using System;

namespace ECMCS.DTO
{
    public class FileDownloadDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Owner { get; set; }
        public string Version { get; set; }
        public bool ReadOnly { get; set; } = true;
        public DateTime ModifiedDate { get; set; } = DateTime.Today;
        public bool IsDone { get; set; } = false;
        public bool IsUploaded { get; set; } = false;
    }
}
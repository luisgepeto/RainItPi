﻿

namespace RainIt.Domain.DTO
{
    public class UploadConstraintParameters
    {
        public int? MaxFileSize { get; set; }
        public int? MaxWidth { get; set; }
        public int? MaxHeight { get; set; }
        public int? MaxNameLength { get; set; }
    }
}

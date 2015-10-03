using System;

namespace RainIt.Domain.Repository
{
    public class UserSettings
    {
        public int UserId { get; set; }
        public int MaxPatternByteCount { get; set; }
        public int MaxPatternPixelHeight { get; set; }
        public int MaxPatternPixelWidth { get; set; }
        public int MaxPatternCountPerRoutine { get; set; }
        public int MaxNumberOfRepetitionsPerPattern { get; set; }
        public virtual User User { get; set; }
    }
}

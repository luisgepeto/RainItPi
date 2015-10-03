namespace RainIt.Domain.DTO
{
    public class UserSettingsDTO
    {
        public int MaxPatternByteCount { get; set; }
        public int MaxPatternPixelHeight { get; set; }
        public int MaxPatternPixelWidth { get; set; }
        public int MaxPatternCountPerRoutine { get; set; }
        public int MaxNumberOfRepetitionsPerPattern { get; set; }
    }
}

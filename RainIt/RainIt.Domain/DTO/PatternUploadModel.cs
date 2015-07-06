using ImageProcessing.Domain;

namespace RainIt.Domain.DTO
{
    public class PatternUploadModel
    {
        public int PatternId { get; set; }
        public string FileName { get; set; }
        public string Base64Image { get; set; }
        public AbsoluteResizeParameters AbsoluteResizeParameters { get; set; }
        public BlackWhiteConversionParameters BlackWhiteConversionParameters { get; set; }
        public ColorRelativeWeight ColorRelativeWeight { get; set; }
    }
}
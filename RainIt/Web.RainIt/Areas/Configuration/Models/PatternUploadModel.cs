using ImageProcessing.Domain;

namespace Web.RainIt.Areas.Configuration.Models
{
    public class PatternUploadModel
    {
        public string FileName { get; set; }
        public string Base64Image { get; set; }
        public AbsoluteResizeParameters AbsoluteResizeParameters { get; set; }
        public BlackWhiteConversionParameters BlackWhiteConversionParameters { get; set; }
        public ColorRelativeWeight ColorRelativeWeight { get; set; }
    }
}
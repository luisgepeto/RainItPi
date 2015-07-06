namespace RainIt.Domain.DTO
{
    public class PatternDTO
    {
        public int PatternId { get; set; }
        public string Base64Image { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public ConversionParameterDTO ConversionParameterDTO { get; set; }
    } 
}

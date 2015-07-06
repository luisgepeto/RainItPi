using RainIt.Domain.Repository;

namespace RainIt.Domain.DTO
{
    public class ConversionParameterDTO
    {
        public double RWeight { get; set; }
        public double GWeight { get; set; }
        public double BWeight { get; set; }
        public double ThresholdPercentage { get; set; }
        public bool IsInverted { get; set; }
    }
}

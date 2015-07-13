
namespace RainIt.Domain.DTO
{
    public class RoutinePatternDTO
    {
        public int RoutinePatternId { get; set; }
        public RoutineDTO RoutineDTO { get; set; }
        public PatternDTO PatternDTO { get; set; }
        public int Repetitions { get; set; }
    }
}

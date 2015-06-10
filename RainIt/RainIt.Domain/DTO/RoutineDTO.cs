using System.Collections.Generic;

namespace RainIt.Domain.DTO
{
    public class RoutineDTO
    {
        public int RoutineId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<RoutinePatternDTO> RoutinePatternDTOs { get; set; }
    }
}

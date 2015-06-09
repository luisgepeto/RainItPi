using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class RoutineList
    {
        public int RoutineId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<RoutinePatternList> RoutinePatterns { get; set; }
    }
}

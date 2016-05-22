
using System.Collections.Generic;


namespace RainIt.Domain.Repository
{
    public class Routine
    {
        public int RoutineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<RoutinePattern> RoutinePatterns { get; set; } 
        public virtual ICollection<Device> Devices { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.Repository
{
    public class SampleRoutine
    {
        public int SampleRoutineId { get; set; }
        public int DeviceId { get; set; }
        public virtual Device Device { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public List<Pattern> Patterns { get; set; }
    }
}

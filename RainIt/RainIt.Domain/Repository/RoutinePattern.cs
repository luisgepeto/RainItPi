using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.Repository
{
    public class RoutinePattern
    {
        public int RoutinePatternId { get; set; }
        public int? RoutineId { get; set; }
        public virtual Routine Routine { get; set; }
        public int? PatternId { get; set; }
        public virtual Pattern Pattern { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}

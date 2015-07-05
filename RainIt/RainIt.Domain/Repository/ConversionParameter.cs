using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.Repository
{
    public class ConversionParameter
    {
        public int PatternId { get; set; }
        public double RWeight { get; set; }
        public double GWeight { get; set; }
        public double BWeight { get; set; }
        public double ThresholdValue { get; set; }
        public bool IsInverted { get; set; }
        public virtual Pattern Pattern { get; set; }
    }
}

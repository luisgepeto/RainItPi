using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class SettingsDTO
    {
        public int MinutesRefreshRate { get; set; }
        public int MillisecondLatchDelay { get; set; }
        public int MillisecondClockDelay { get; set; }
    }
}

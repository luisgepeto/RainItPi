using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.Repository
{
    public class DeviceSettings
    {
        public int DeviceId { get; set; }
        public int MinutesRefreshRate { get; set; }
        public int MillisecondLatchDelay { get; set; }
        public int MillisecondClockDelay { get; set; }
        public virtual Device Device { get; set; }
    }
}

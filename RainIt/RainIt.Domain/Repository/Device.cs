

using System;
using System.Collections.Generic;

namespace RainIt.Domain.Repository
{
    public class Device
    {
        public int DeviceId { get; set; }
        public virtual DeviceInfo DeviceInfo { get; set; }
        //public virtual DeviceCredential DeviceCredential { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Routine> Routines { get; set; }
    }
}

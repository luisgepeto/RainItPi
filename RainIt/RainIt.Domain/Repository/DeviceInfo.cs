using System;

namespace RainIt.Domain.Repository
{
    public class DeviceInfo
    {
        public int DeviceId { get; set; }
        public Guid Identifier { get; set; }
        public string Serial { get; set; }
        public virtual Device Device { get; set; }
        public DateTime RegisteredUTCDate { get; set; }
    }
}

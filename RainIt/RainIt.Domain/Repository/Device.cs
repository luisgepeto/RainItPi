

using System;

namespace RainIt.Domain.Repository
{
    public class Device
    {
        public int DeviceId { get; set; }
        public virtual DeviceInfo DeviceInfo { get; set; }
        public virtual DeviceCredential DeviceCredential { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int? RoutineId { get; set; }
        public virtual Routine Routine { get; set; }
    }
}

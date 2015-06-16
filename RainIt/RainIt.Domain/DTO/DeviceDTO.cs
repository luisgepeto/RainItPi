
using System;

namespace RainIt.Domain.DTO
{
    public class DeviceDTO
    {
        public int DeviceId { get; set; }
        public Guid Identifier { get; set; }
        public string Serial { get; set; }
        public int? RoutineId { get; set; }
    }
}

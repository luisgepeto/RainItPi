
using System;
using System.Collections.Generic;

namespace RainIt.Domain.DTO
{
    public class DeviceDTO
    {
        public int DeviceId { get; set; }
        public Guid Identifier { get; set; }
        public string Serial { get; set; }
        public List<RoutineDTO> RoutineDTOs { get; set; }
    }
}

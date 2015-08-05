﻿using System;
using System.Collections.Generic;

namespace RainIt.Domain.DTO
{
    public class RoutineDTO
    {
        public int RoutineId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<RoutinePatternDTO> RoutinePatternDTOs { get; set; }
        public List<DeviceDTO> DeviceDTOs { get; set; }
        public DateTime? SampleTimeStamp { get; set; }
    }
}

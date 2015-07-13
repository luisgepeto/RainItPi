using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class RoutineUploadModel
    {
        public int RoutineId { get; set; }
        public List<RoutinePatternDTO> RoutinePatternDTOList { get; set; }
        public List<Guid> DeviceIdentifierList { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsTest { get; set; }
    }
}

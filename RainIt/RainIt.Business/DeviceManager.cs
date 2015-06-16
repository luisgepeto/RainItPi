using System.Collections.Generic;
using System.Linq;
using RainIt.Domain.DTO;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;

namespace RainIt.Business
{
    public class DeviceManager : IDeviceManager
    {
        public IRainItContext RainItContext { get; set; }
        public DeviceManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        public StatusMessage AddUserDevice(DeviceDTO device)
        {
            if (!IsSerialAvailable(device.Serial)) return StatusMessage.WriteError("The selected serial is already in use");
            return StatusMessage.WriteMessage("The selected device was successfully added");
        }

        private bool IsSerialAvailable(string serial)
        {
            return !RainItContext.DeviceSet.Any(d => d.DeviceInfo.Serial == serial);
        }

        public List<DeviceDTO> GetUserDevices()
        {
            return RainItContext.UserDeviceSet.Select(d => new DeviceDTO()
            {
                DeviceId = d.DeviceId,
                Identifier = d.DeviceInfo.Identifier,
                RoutineId = d.RoutineId
            }).ToList();
        }
    }
}

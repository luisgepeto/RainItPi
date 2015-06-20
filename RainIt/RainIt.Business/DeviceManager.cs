using System;
using System.Collections.Generic;
using System.Linq;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
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
            return AddDevice(device);
        }

        private bool IsSerialAvailable(string serial)
        {
            return !RainItContext.DeviceSet.Any(d => d.DeviceInfo.Serial == serial);
        }

        private StatusMessage AddDevice(DeviceDTO device)
        {
            try
            {
                var deviceToAdd = new Device();
                var deviceInfoForDevice = new DeviceInfo()
                {
                    Identifier = Guid.NewGuid(),
                    RegisteredUTCDate = DateTime.UtcNow,
                    Serial = device.Serial
                };
                var user = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username);
                deviceToAdd.DeviceInfo = deviceInfoForDevice;
                deviceToAdd.User = user;

                RainItContext.DeviceSet.Add(deviceToAdd);
                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("The device was successfully saved");
            }
            catch (Exception e)
            {
                return StatusMessage.WriteMessage("An unexpected error occurred while trying to save the device");
            }
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

        public Guid GetDeviceGuid(string serial)
        {
            return RainItContext.DeviceSet.Single(d => d.DeviceInfo.Serial == serial).DeviceInfo.Identifier;
        }

        public bool ValidateDevice(string serial)
        {
            var device = RainItContext.DeviceSet.SingleOrDefault(d => d.DeviceInfo.Serial == serial);
            return device != null;
        }
    }
}

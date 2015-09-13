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
            Device outDevice;
            if (!TryGetDevice(device.Identifier, out outDevice)) return StatusMessage.WriteError("The selected device does not exist");
            if (!IsDeviceAvailable(device.Identifier)) return StatusMessage.WriteError("The selected device is already in use");
            return AssignToUser(outDevice, device.Name);
        }

        private bool TryGetDevice(Guid identifier, out Device outDevice)
        {
            outDevice = RainItContext.DeviceSet.SingleOrDefault(d => d.DeviceInfo.Identifier == identifier);
            return outDevice != null;
        }

        private bool TryGetDevice(int deviceId, out Device outDevice)
        {
            outDevice = RainItContext.DeviceSet.SingleOrDefault(d => d.DeviceId == deviceId);
            return outDevice != null;
        }

        private StatusMessage AssignToUser(Device device, string name)
        {
            var user = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username);
            device.User = user;
            device.Name = name;
            device.DeviceInfo.ActivatedUTCDate = DateTime.UtcNow;
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The device was successfully assigned to the current user");
            
        }

        public StatusMessage AddDevice(DeviceDTO device)
        {
            if (!IsSerialFormatValid(device.Serial)) return StatusMessage.WriteError("The selected serial is not in the correct format");
            return AddDeviceToDatabase(device);
        }

        private bool IsSerialFormatValid(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(test, @"^[0-9a-fA-F]{16}$");
        }
        private bool IsSerialAvailable(string serial)
        {
            return !RainItContext.DeviceSet.Any(d => d.DeviceInfo.Serial == serial);
        }

        private StatusMessage AddDeviceToDatabase(DeviceDTO device)
        {
            var deviceToAdd = new Device();
            var deviceInfoForDevice = new DeviceInfo()
            {
                Identifier = Guid.NewGuid(),
                Serial = device.Serial
            };
            deviceToAdd.DeviceInfo = deviceInfoForDevice;
                
            RainItContext.DeviceSet.Add(deviceToAdd);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The device was successfully saved");
        }

        public List<DeviceDTO> GetUserDevices()
        {
            var allUserDevices = RainItContext.UserDeviceSet;
            return ToDeviceDTOList(allUserDevices);
        }

        public List<DeviceDTO> GetAllDevices()
        {
            var allDevices =  RainItContext.DeviceSet;
            return ToDeviceDTOList(allDevices);
        }

        private List<DeviceDTO> ToDeviceDTOList(IQueryable<Device> deviceQueryable)
        {
            return deviceQueryable.Select(d => new DeviceDTO()
            {
                DeviceId = d.DeviceId,
                Identifier = d.DeviceInfo.Identifier,
                Name = d.Name,
                RoutineDTOs = d.Routines.Where(r => r.RoutinePatterns.Any()).Select(r => new RoutineDTO()
                {
                    Name = r.Name,
                    RoutineId = r.RoutineId
                }).ToList()
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
        public bool IsDeviceAvailable(Guid identifier)
        {
            var device = RainItContext.DeviceSet.SingleOrDefault(d => d.DeviceInfo.Identifier == identifier);
            if (device == null) return false;
            if (device.UserId.HasValue) return false;
            if (device.DeviceInfo.IsAlreadyActive) return false;
            return true;
        }

        public SettingsDTO GetCurrentSettings()
        {
            return RainItContext.DeviceSettingsSet.Select(s => new SettingsDTO()
            {
                MinutesRefreshRate = s.MinutesRefreshRate,
                MillisecondClockDelay = s.MillisecondClockDelay,
                MillisecondLatchDelay = s.MillisecondLatchDelay
            }).SingleOrDefault();
        }

        public StatusMessage EditUserDevice(int deviceId, string newDeviceName)
        {
            Device outDevice;
            if (!TryGetDevice(deviceId, out outDevice))
                return StatusMessage.WriteError("The selected device does not exist for the current user");
            var currentName = outDevice.Name;
            if (currentName != newDeviceName)
            {
                if(!IsNameAvailable(newDeviceName))
                    return StatusMessage.WriteError("The selected device name is already in use");
                outDevice.Name = newDeviceName;
                RainItContext.SaveChanges();
            }
            return StatusMessage.WriteMessage("Successfully updated the current device name");
        }

        private bool IsNameAvailable(string newDeviceName)
        {
            return !RainItContext.UserDeviceSet.Any(d => d.Name == newDeviceName);
        }
    }
}

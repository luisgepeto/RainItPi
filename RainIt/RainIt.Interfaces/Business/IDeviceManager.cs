using System;
using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IDeviceManager
    {
        StatusMessage AddUserDevice(DeviceDTO device);
        StatusMessage AddDevice(DeviceDTO device);
        List<DeviceDTO> GetUserDevices();
        List<DeviceDTO> GetAllDevices();
        Guid GetDeviceGuid(string serial);
        bool ValidateDevice(string serial);
        bool IsDeviceAvailable(Guid identifier);
        StatusMessage EditUserDevice(int deviceId, string newDeviceName);
    }
}

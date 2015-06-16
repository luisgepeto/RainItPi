using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IDeviceManager
    {
        StatusMessage AddUserDevice(DeviceDTO device);
        List<DeviceDTO> GetUserDevices();
    }
}

using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IUserManager
    {
        List<UserDTO> GetAllUsers();
        UserDTO GetDetails(int userId);
        UserSettingsDTO GetSettings(int userId);
        List<DeviceDTO> GetDevices(int userId);
    }
}

using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IUserManager
    {
        List<UserDTO> GetAllUsers();
        List<UserDTO> GetDetails(int userId);
        List<DeviceDTO> GetDevices(int userId);
    }
}

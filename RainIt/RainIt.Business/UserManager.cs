using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kendo.Mvc.Extensions;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;

namespace RainIt.Business
{
    public class UserManager : IUserManager
    {
        public IRainItContext RainItContext { get; set; }

        public UserManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;z
        }

        public List<UserDTO> GetAllUsers()
        {
            return RainItContext.UserSet.Select(u => new UserDTO()
            {
                UserId = u.UserId,
                Username = u.Username
            }).ToList();
        }

        public List<UserDTO> GetDetails(int userId)
        {
            return RainItContext.UserSet.Select(u => new UserDTO()
            {
                UserId = u.UserId,
                Username = u.Username,
                Email = u.Email,
                UserSettings = new UserSettingsDTO()
                {
                    MaxPatternPixelWidth = u.UserSettings.MaxPatternPixelWidth,
                    MaxPatternByteCount = u.UserSettings.MaxPatternByteCount,
                    MaxPatternPixelHeight = u.UserSettings.MaxPatternPixelHeight,
                    MaxPatternCountPerRoutine = u.UserSettings.MaxPatternCountPerRoutine,
                    MaxNumberOfRepetitionsPerPattern = u.UserSettings.MaxNumberOfRepetitionsPerPattern
                }
            }).ToList();
        }

        public List<DeviceDTO> GetDevices(int userId)
        {
            var userDevices =
                RainItContext.DeviceSet.Where(d => d.UserId == userId)
                    .Include(typeof (DeviceSettings).Name)
                    .Include(typeof (DeviceInfo).Name);

            return userDevices.Select(d => new DeviceDTO()
            {
                DeviceId = d.DeviceId,
                Serial = d.DeviceInfo.Serial,
                Identifier = d.DeviceInfo.Identifier,
                Name = d.Name,
                DeviceSettings = new DeviceSettingsDTO()
                {
                    MillisecondClockDelay = d.DeviceSettings.MillisecondClockDelay,
                    MillisecondLatchDelay = d.DeviceSettings.MillisecondLatchDelay,
                    MinutesRefreshRate = d.DeviceSettings.MinutesRefreshRate
                }
            }).ToList();
        }
    }
}

using System;
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
            RainItContext = rainItContext;
        }

        public List<UserDTO> GetAllUsers()
        {
            return RainItContext.UserSet.Select(u => new UserDTO()
            {
                UserId = u.UserId,
                Username = u.Username
            }).ToList();
        }

        public UserDTO GetDetails(int userId)
        {
            return RainItContext.UserSet.Where(u => u.UserId == userId).Select(u => new UserDTO()
            {
                UserId = u.UserId,
                Email = u.Email,
                Username = u.Username,
                UserInfo = new UserInfoDTO()
                {
                    FirstName = u.UserInfo.FirstName,
                    LastName = u.UserInfo.LastName,
                    BirthDate = u.UserInfo.BirthDate
                },
                Address = new AddressDTO()
                {
                    AddressLine1 = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().AddressLine1 : String.Empty,
                    AddressLine2 = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().AddressLine2 : String.Empty,
                    City = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().City : String.Empty,
                    Country = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().Country : String.Empty,
                    State = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().State : String.Empty,
                    PhoneNumber = u.Addresses.FirstOrDefault() != null ? u.Addresses.FirstOrDefault().PhoneNumber : String.Empty,
                }
            }).SingleOrDefault();
        }

        public UserSettingsDTO GetSettings(int userId)
        {
            return RainItContext.UserSet.Where(u => u.UserId == userId).Select(u => new UserSettingsDTO()
            {
                MaxPatternPixelWidth = u.UserSettings.MaxPatternPixelWidth,
                MaxPatternByteCount = u.UserSettings.MaxPatternByteCount,
                MaxPatternPixelHeight = u.UserSettings.MaxPatternPixelHeight,
                MaxPatternCountPerRoutine = u.UserSettings.MaxPatternCountPerRoutine,
                MaxNumberOfRepetitionsPerPattern = u.UserSettings.MaxNumberOfRepetitionsPerPattern
            }).SingleOrDefault();
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

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

        public UserDTO GetInformation(int userId)
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

        public StatusMessage EditInformation(UserDTO user)
        {
            if(String.IsNullOrEmpty(user.Email)) return StatusMessage.WriteError("The user e-mail cannot be blank");
            if(String.IsNullOrEmpty(user.Username)) return StatusMessage.WriteError("The username cannot be blank");
            User userToEdit;
            if(!TryGetUser(user.UserId, out userToEdit)) return StatusMessage.WriteError("The selected user id does not exist");
            if(!IsUsernameAvailable(user.Username)) return  StatusMessage.WriteError("The selected username is already in use");
            if (!IsEmailAvailable(user.Email)) return StatusMessage.WriteError("The selected email is already in use");
            return EditUser(userToEdit, user);
        }

        private bool TryGetUser(int userId, out User userToEdit)
        {
            userToEdit = RainItContext.UserSet.SingleOrDefault(u => u.UserId == userId);
            return userToEdit != null;
        }

        private bool IsUsernameAvailable(string username)
        {
            return
                !RainItContext.UserSet.Any(
                    u => String.Compare(u.Username, username, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        private bool IsEmailAvailable(string email)
        {
            return
                !RainItContext.UserSet.Any(
                    u => String.Compare(u.Email, email, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        private StatusMessage EditUser(User userToEdit, UserDTO newUserInfo)
        {
            if (userToEdit.Username != newUserInfo.Username)
                userToEdit.Username = newUserInfo.Username;
            if (userToEdit.Email != newUserInfo.Email)
                userToEdit.Email = newUserInfo.Email;
            RainItContext.UserSet.Attach(userToEdit);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("Successfully saved changes");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Util;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;
using Web.Security.Interfaces;

namespace RainIt.Business
{
    public class AccountManager : IAccountManager
    {
        public IRainItContext RainItContext { get; set; }
        public ICryptoServiceManager CryptoServiceManager { get; set; } 

        public AccountManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        public StatusMessage Register(Registration registration)
        {
            if (!IsUsernameAvailable(registration.User.Username))
                return StatusMessage.WriteError("The selected username is already in use.");
            if (!IsEmailAvailable(registration.User.Email))
                return StatusMessage.WriteError("The selected email address is already in use.");
            if(!DoPasswordsMatch(registration.User.Password, registration.User.PasswordConfirmation))
                return StatusMessage.WriteError("The selected email address is already in use.");

            return SaveToDatabase(registration);
        }
        
        public bool IsUsernameAvailable(string username)
        {
            username = username.Trim();
            var isUserNameAvailable =  !RainItContext.UserSet.Any(u => u.Username.Equals(username));
            return isUserNameAvailable;
        }

        public bool IsEmailAvailable(string email)
        {
            email = email.Trim();
            return !RainItContext.UserSet.Any(u => u.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        private bool DoPasswordsMatch(string password, string passwordConfirmation)
        {
            return String.CompareOrdinal(password, passwordConfirmation) == 0;
        }

        private StatusMessage SaveToDatabase(Registration registration)
        {
            var userToAdd = registration.User.ConvertTo(new User());
            var passwordToAdd = CreatePasswordFrom(registration.User);
            var userInfoToAdd = registration.UserInfo.ConvertTo(new UserInfo());
            var userSettingsToAdd = GetDefaultUserSettings();
            var addressToAdd = registration.Address.ConvertTo(new Address());
            var deviceToLink = RainItContext.DeviceSet.Single(d => d.DeviceInfo.Identifier == registration.DeviceInfo.Identifier);
            deviceToLink.DeviceInfo.ActivatedUTCDate = DateTime.UtcNow;
            deviceToLink.Name = "Initial RainIt device for "+registration.User.Username;
            userToAdd.UserInfo = userInfoToAdd;
            userToAdd.Password = passwordToAdd;
            userToAdd.Addresses = new List<Address>() {addressToAdd};
            userToAdd.RoleId = RainItContext.RoleSet.Single(r => r.Name == "customer").RoleId;
            userToAdd.Devices = new List<Device> {deviceToLink};
            userToAdd.UserSettings = userSettingsToAdd;
            RainItContext.UserSet.Add(userToAdd);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("You were successfully registered.");
        }

        private UserSettings GetDefaultUserSettings()
        {
            return new UserSettings()
            {
                MaxNumberOfRepetitionsPerPattern = 5,
                MaxPatternByteCount = 153600,
                MaxPatternCountPerRoutine = 10,
                MaxPatternPixelWidth = 200,
                MaxPatternPixelHeight = 200
            };
        }

        private Password CreatePasswordFrom(UserDTO userToAdd)
        {
            var currentSalt = CryptoServiceManager.CreateRandomSalt();
            var concatenatedPass = currentSalt + userToAdd.Password;
            var currentHash = CryptoServiceManager.GetHashFrom(concatenatedPass);

            var password = new Password()
            {
                Salt = currentSalt,
                Hash = currentHash
            };
            return password;
        }

        public StatusMessage Authenticate(Login login)
        {
            login.Username = login.Username.Trim();
            var user = RainItContext.UserSet.SingleOrDefault(u => u.Username.Equals(login.Username));
            if(user == null) return StatusMessage.WriteError("Your username and password combination is incorrect. Please review your credentials.");
                
            var concatenatedPass = user.Password.Salt + login.Password;
            var generatedHash = CryptoServiceManager.GetHashFrom(concatenatedPass);
            var arePasswordsEqual = generatedHash.Equals(user.Password.Hash);
                
            return arePasswordsEqual? StatusMessage.WriteMessage("You were successfully logged in") :
                StatusMessage.WriteError("Your username and password combination is incorrect. Please review your credentials.");
           
        }

        public string GetRoleFor(string username)
        {
            var roleForUser =
                RainItContext.UserSet.Single(u => u.Username == username).Role.Name;

            return roleForUser;
        } 
    }
}

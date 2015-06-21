using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Common.Util;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;
using User = RainIt.Domain.DTO.User;


namespace RainIt.Business
{
    public class AccountManager : IAccountManager
    {
        public IRainItContext RainItContext { get; set; }

        public AccountManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        public User GetCurrentUsername()
        {
            return RainItContext.CurrentUser;
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
            try
            {
                var userToAdd = registration.User.ConvertTo(new Domain.Repository.User());
                var passwordToAdd = CreatePasswordFrom(registration.User);
                var userInfoToAdd = registration.UserInfo.ConvertTo(new Domain.Repository.UserInfo());
                var addressToAdd = registration.Address.ConvertTo(new Domain.Repository.Address());
                var deviceToLink = RainItContext.DeviceSet.Single(d => d.DeviceInfo.Identifier == registration.DeviceInfo.Identifier);
                deviceToLink.DeviceInfo.ActivatedUTCDate = DateTime.UtcNow;

                userToAdd.UserInfo = userInfoToAdd;
                userToAdd.Password = passwordToAdd;
                userToAdd.Addresses = new List<Domain.Repository.Address>() {addressToAdd};
                userToAdd.Roles = RainItContext.RoleSet.Where(r => r.Name == "customer").ToList();
                userToAdd.Devices = new List<Device> {deviceToLink};

                RainItContext.UserSet.Add(userToAdd);

                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("You were successfully registered.");
            }
            catch (Exception ex)
            {
                return StatusMessage.WriteError("An unexpected error occurred. Please try again.");
            }
        }

        private Password CreatePasswordFrom(RainIt.Domain.DTO.User userToAdd)
        {
            var currentSalt = CreateRandomSalt();
            var concatenatedPass = currentSalt + userToAdd.Password;
            var currentHash = GetHashFrom(concatenatedPass);

            var password = new Password()
            {
                Salt = currentSalt,
                Hash = currentHash
            };
            return password;
        }

        private string CreateRandomSalt()
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[16];
            rng.GetBytes(buff);
            var currentSalt = Convert.ToBase64String(buff);
            return currentSalt;
        }

        private string GetHashFrom(string concatenatedPass)
        {
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider(); 
            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(concatenatedPass);
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            string base64 = Convert.ToBase64String(bytHash);
            return base64;
        }
        

        public StatusMessage Authenticate(Login login)
        {
            try
            {
                login.Username = login.Username.Trim();
                var user = RainItContext.UserSet.SingleOrDefault(u => u.Username.Equals(login.Username));
                if(user == null) return StatusMessage.WriteError("Your username and password combination is incorrect. Please review your credentials.");
                
                var concatenatedPass = user.Password.Salt + login.Password;
                var generatedHash = GetHashFrom(concatenatedPass);
                var arePasswordsEqual = generatedHash.Equals(user.Password.Hash);
                
                return arePasswordsEqual? StatusMessage.WriteMessage("You were successfully logged in") :
                    StatusMessage.WriteError("Your username and password combination is incorrect. Please review your credentials.");
            }
            catch (Exception ex)
            {
               return StatusMessage.WriteError("An unexpected error has occurred. Please try again later.");
            }
        }

        public List<string> GetRolesFor(string username)
        {
            var rolesForUser =
                RainItContext.UserSet.Single(u => u.Username == username).Roles.Select(r => r.Name).ToList();
                    
            return rolesForUser;
        } 
    }
}

using System;
using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IAccountManager
    {

        StatusMessage Register(Registration registration);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        string GetRoleFor(string username);
        StatusMessage Authenticate(Login login);
        
    }
}

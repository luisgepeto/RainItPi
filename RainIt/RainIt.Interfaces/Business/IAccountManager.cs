
using System.Collections.Generic;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;

namespace RainIt.Interfaces.Business
{
    public interface IAccountManager
    {
        StatusMessage Register(Registration registration);
        bool IsUsernameAvailable(string username);
        bool IsEmailAvailable(string email);
        StatusMessage Authenticate(Login login);
        List<string> GetRolesFor(string username);
    }
}

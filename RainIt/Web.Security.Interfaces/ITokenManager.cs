
using System.Security.Claims;

namespace Web.Security.Interfaces
{
    public interface ITokenManager
    {
        string CreateJwtToken(string userName);
        ClaimsPrincipal ValidateJwtToken(string jwtToken);
    }
}

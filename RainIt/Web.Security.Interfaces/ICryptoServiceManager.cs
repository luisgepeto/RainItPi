

namespace Web.Security.Interfaces
{
    public interface ICryptoServiceManager
    {
        string CreateRandomSalt();
        string GetHashFrom(string concatenatedPass);
    }
}

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
    }
}

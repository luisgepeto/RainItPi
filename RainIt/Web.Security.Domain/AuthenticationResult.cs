using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Security.Domain
{
    public class AuthenticationResult
    {
        public string SecurityToken { get; set; }
        public DateTime? TokenExpirationUtcTime { get; set; }
        public string ErrorMessage { get; set; }
        public LoginStatus LoginStatus { get; set; }
    }
}

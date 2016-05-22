using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Security.Domain
{
    public enum LoginStatus
    {
        [Description("Authorized user")]
        ValidUser = 1,
        [Description("Insufficient user credentials")]
        InsufficientCredentials = 3,
        [Description("Invalid user credentials")]
        InvalidCredentials = 4,
        [Description("User account locked. This account will be locked for a 15 minute period due to 10 or more unsuccessful login attempts.")]
        AccountLocked = 5,
        [Description("Account deactivated due to lack of use. Please contact an Administator to reactivate.")]
        AccountInactive = 6
    }
}

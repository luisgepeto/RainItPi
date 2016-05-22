using System.Collections.Generic;

namespace RainIt.Domain.Repository
{
    public class User
    {
        public int  UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserInfo UserInfo { get; set; }
        public virtual Password Password { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Pattern> Patterns { get; set; } 
        public virtual ICollection<Routine> Routines { get; set; } 
        public virtual ICollection<RoutinePattern> RoutinePatterns { get; set; } 
        public virtual ICollection<Device> Devices { get; set; }
        public virtual UserSettings UserSettings { get; set; }

    }
}


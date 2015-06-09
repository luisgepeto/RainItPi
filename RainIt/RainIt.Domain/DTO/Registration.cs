using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.Repository;

namespace RainIt.Domain.DTO
{
    public class Registration
    {
        public User User { get; set; }
        public Address Address { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}

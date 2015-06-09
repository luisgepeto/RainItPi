using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class Login
    {
        [Required(ErrorMessage="Please enter your username")]
        public string Username { get; set; }
        [Required(ErrorMessage="Please enter your password")]
        [DataType(DataType.Password)]        
        public string Password { get; set; }
    }
}

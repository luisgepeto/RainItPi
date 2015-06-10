using System.ComponentModel.DataAnnotations;

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

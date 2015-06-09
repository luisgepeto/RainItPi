

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace RainIt.Domain.DTO
{
    public class User
    {
        public int  UserId { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("username")]
        [Editable(true)]
        [Remote("IsUsernameAvailable", "Account")]
        public string Username { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("email address")]
        [EmailAddress(ErrorMessage="Please enter a valid {0}")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, MinimumLength = 6)]
        [Editable(true)]
        [Remote("IsEmailAvailable", "Account")]
        public string Email { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("password")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage="The {0} field must be between 6 and 20 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("password confirmation")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage="The supplied passwords do not match")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage="The {0} field must be between 6 and 20 characters.")]
        public string PasswordConfirmation { get; set; }
    }
}

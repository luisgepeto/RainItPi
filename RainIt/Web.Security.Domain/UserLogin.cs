
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Security.Domain
{
    public class UserLogin
    {
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]    
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class UserInfo
    {
        public int UserId { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("first name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("last name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("date of birth")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("gender")]
        [StringLength(10)]
        public string Gender { get; set; }
    }
}

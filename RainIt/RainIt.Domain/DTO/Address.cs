using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RainIt.Domain.DTO
{
    public class Address
    {
        public int AddressId { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("country")]
        [StringLength(20)]
        public string Country { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("state")]
        [StringLength(20)]
        public string State { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("city")]
        [StringLength(20)]
        public string City { get; set; }
        [Required(ErrorMessage="The {0} field is required")]
        [DisplayName("address line 1")]
        [StringLength(80)]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}

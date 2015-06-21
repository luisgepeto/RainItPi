using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RainIt.Domain.DTO
{
    public class DeviceInfoDTO
    {
        [Required(ErrorMessage = "The {0} field is required")]
        [DisplayName("device identifier")]
        [Remote("IsDeviceAvailable", "Account")]
        public Guid? Identifier { get; set; }
    }
}

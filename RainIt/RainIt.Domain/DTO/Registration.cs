using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using RainIt.Domain.Repository;

namespace RainIt.Domain.DTO
{
    public class Registration
    {
        public User User { get; set; }
        public DeviceInfoDTO DeviceInfo { get; set; }
        public Address Address { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}

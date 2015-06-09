using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.Repository
{
    public class Password
    {
        public int UserId { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }
        public virtual User User { get; set; }
    }
}

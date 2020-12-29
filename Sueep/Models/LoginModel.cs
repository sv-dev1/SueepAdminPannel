using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class LoginModel
    {
        [Required]
        public String Email { get; set; }
        [Required]
        public string password { get; set; }
   

        public string LoginError { get; set; }
    }
}

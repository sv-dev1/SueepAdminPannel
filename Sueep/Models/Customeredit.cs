using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Customeredit
    {
        [Key]
        public int RegisterId { get; set; }
       [Required]
        public string Email { get; set; }
        [Required]
        public string PersonName { get; set; }
        [Required]
        public string City { get; set; }
    }
}

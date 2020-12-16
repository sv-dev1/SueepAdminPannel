using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class customer
    {
        public int ResgisterId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }
       
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonName { get; set; }
        public string Country { get; set; }
      
        public string City { get; set; }
    }
}

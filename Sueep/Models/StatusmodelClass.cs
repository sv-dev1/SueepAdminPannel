using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class StatusmodelClass
    {
        [Key]
        public int Id { get; set; }
       
        public string Email { get; set; }

        public string Status { get; set; }
        public string Servicestatus { get; set; }
        public string dateofservice { get; set; }
        public string timeofservice { get; set; }
        public string Gender { get; set; }
    
        public string PersonName { get; set; }


        public int PesonalInfoId { get; set; }
      
        public string FirstName { get; set; }

        public string Amount { get; set; }
        
        public string LastName { get; set; }
        //[Required]



        public string Phone { get; set; }
        // [Required]
        public string ZipCode { get; set; }


        public string ServiceTime { get; set; }

        public int SueeperId { get; set; }
    }
}

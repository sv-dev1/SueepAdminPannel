using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Sueeper
    {

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
       

        public string Phone { get; set; }
        
        public string City { get; set; }
       
        public string Socialsecuritynumber { get; set; }

        public Nullable<System.DateTime> createddate { get; set; }
        public string IsRole { get; set; }
        
        public string Do_you_have_car { get; set; }
       
        public string Can_you_buy_cleaning_products { get; set; }
      
        public string Zipcode { get; set; }
        public string Password { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string IsBusy { get; set; }
    }
}

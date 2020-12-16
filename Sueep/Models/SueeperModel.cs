using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class SueeperModel
    {
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Socialsecuritynumber { get; set; }
        public Nullable<System.DateTime> createddate { get; set; }
        public string IsRole { get; set; }
        [Required]
        public string Do_you_have_car { get; set; }
        [Required]
        public string Can_you_buy_cleaning_products { get; set; }
        [Required]
        public string Zipcode { get; set; }
       
        public string Password { get; set; }
        public string lng { get; set; }
        public string lat { get; set; }
        public string IsBusy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Services
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public Nullable<int> NoOfBedrooms { get; set; }
        public Nullable<int> NoOfBathrooms { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> CleaningTypeId { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Timetble
    {
        [Key]
        public int Id { get; set; }
        public string TimeOfService { get; set; }
        public string DateOfService { get; set; }
        public string ExtraService { get; set; }
        public Nullable<int> PersonalInfoId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Assignsueeper
    {
        [Key]
        public int JobId { get; set; }
        public Nullable<int> PersonaLInfoId { get; set; }
        public Nullable<int> sueeperId { get; set; }
        public Nullable<double> Rate { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> Ratingdate { get; set; }
        public string Status { get; set; }
        public string journeystatus { get; set; }
        public string JobStatus { get; set; }
        public Nullable<double> userrating { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsBusy { get; set; }
        public string Dateofservice { get; set; }
        public string Timeofservice { get; set; }
    }
}

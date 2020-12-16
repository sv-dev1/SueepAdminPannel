using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Paymenttbl
    {
        [Key]
        public int PaymentId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string PaymentAmount { get; set; }
        public string JobStatus { get; set; }
    }
}

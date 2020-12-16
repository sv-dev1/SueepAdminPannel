using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class Servicestatusclass
    {
        [Key]
        public int StatusId { get; set; }
        public int serviceid { get; set; }
        public int SueeperId { get; set; }
        public string Servicestatus { get; set; }
    }
}

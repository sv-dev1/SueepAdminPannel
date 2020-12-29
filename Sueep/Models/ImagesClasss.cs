using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class ImagesClasss
    {
        [Key]
        public int PictureId { get; set; }
        public string picturePath { get; set; }
        public Nullable<System.DateTime> img_date { get; set; }
        public Nullable<int> SueeperId { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public Nullable<int> CommercialUserID { get; set; }
        public string Message { get; set; }
        public string Imageurl { get; set; }
        public string Pic_val { get; set; }
    }
}

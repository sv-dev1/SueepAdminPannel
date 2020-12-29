using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class reportclass
    {
        public int serviceid { get; set; }
        public int sueeperid { get; set; }
        public string SueeperName { get; set; }
        public string ServiceName { get; set; }
        public string dateofsevices { get; set; }
        public string Timeofservices { get; set; }
        public string status { get; set; }

    }


    public class GetImagelist
    {
        public int PictureId { get; set; }
        public string picturePath { get; set; }
        public Nullable<System.DateTime> img_date { get; set; }
        public Nullable<int> SueeperId { get; set; }
        public Nullable<int> ServiceID { get; set; }
        public string journeyStatus { get; set; }
        public string P_Id { get; set; }
        public string Comment { get; set; }

        public string Jobstatus { get; set; }
        // public Nullable<int> CommercialUserID { get; set; }
        // public string Message { get; set; }
        public string Imageurl { get; set; }
    }
}

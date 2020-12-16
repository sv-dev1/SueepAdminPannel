using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Models
{
    public class seviecemodelclass
    {

       

            public int Id { get; set; }
            public string Email { get; set; }
            //public string Password { get; set; }
            //public decimal ToatalAmount { get; set; }
            //public Nullable<System.DateTime> JoinDate { get; set; }
            //public string userType { get; set; }
            public string Status { get; set; }
            public string jobstatus { get; set; }

            public string dateofservice { get; set; }
            //public string Dateofservice { get; set; }
            public string timeofservice { get; set; }
            public string Gender { get; set; }
            public string PhoneNumber { get; set; }
            public string PersonName { get; set; }
            //public string CleaningTypeId { get; set; }
            //public string Country { get; set; }
            //public string IsRole { get; set; }
            //public string City { get; set; }

            public int PesonalInfoId { get; set; }
            //[Required]
            public string FirstName { get; set; }

            public string Amount { get; set; }
            //[Required]
            public string LastName { get; set; }
            //[Required]



            public string Phone { get; set; }
            // [Required]
            public string ZipCode { get; set; }
            //[Required]
            //  public int ?NoOfBedrooms { get; set; }
            // [Required]
            //  public int? NoOfBathrooms { get; set; }

            //  public string Address { get; set; }
            //  [Required]
            // public string State { get; set; }

            // [Required]
            //public int? PersonalInfoId { get; set; }
            // public string TimeOfService { get; set; }
            // [Required]
            // public string DateOfService { get; set; }
            // public string ExtraService { get; set; }
            // [Required]
            // public Nullable<int> sueeperId { get; set; }
            // public double? Rate { get; set; }
            // public string picturePath { get; set; }
            //public int? BedroomCount { get; set; }
            //  public string CustomerLocation { get; set; }
            public string ServiceTime { get; set; }

            public int SueeperId { get; set; }
            //  public int ?BathroomCount { get; set; }
        }
    }

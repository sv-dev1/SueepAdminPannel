using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sueep.Models;

namespace Sueep.Models
{
    public class Admincontext:DbContext
    {

        public Admincontext(DbContextOptions<Admincontext>options):base(options)
        {

        }
        public DbSet <Sueeper> SueeperInfo { get; set; }

        public DbSet<Assignsueeper> AssinSueeper { get; set; }

        public DbSet<Services> PersonalInfo { get; set; }

        public DbSet<Addressc> AddressInfo { get; set; }

        public DbSet<Timetble> TimeDateInfo { get; set; }

        public DbSet<Paymenttbl> PaymentTbl { get; set; }
        public DbSet<Users>UserRegistration { get; set; }

        public DbSet<Servicestatusclass> Servicestatus { get; set; }
    }
}

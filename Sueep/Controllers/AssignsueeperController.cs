using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sueep.Helpers;
using Sueep.Models;
using static Sueep.Models.Getmodel;

namespace Sueep.Controllers
{
    public class AssignsueeperController : Controller
    {
        public PaginatedList<Getmodel> getModel { get; set; }


        private readonly Admincontext db;
        public AssignsueeperController(Admincontext Db)
        {
            db = Db;
        }
        //[HttpGet]

        public async Task<IActionResult> GetServices(AssignedSueeper obj, string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            var serviceslist = (from details in db.PersonalInfo
                                join Add in db.AddressInfo on details.Id equals Add.PersonalInfoId
                                join TimeP in db.TimeDateInfo on details.Id equals TimeP.PersonalInfoId
                                join paytbl in db.PaymentTbl on details.Id equals paytbl.ServiceId

                                select new Getmodel
                                {
                                    Id = details.Id,
                                    FirstName = details.FirstName,
                                    LastName = details.LastName,

                                    Phone = details.Phone,
                                    ZipCode = details.ZipCode,
                                    dateofservice = TimeP.DateOfService,
                                    timeofservice = TimeP.TimeOfService,
                                    Email = details.Email,


                                }).OrderByDescending(m => m.Id);
            var sueeperList = db.SueeperInfo;
            ViewBag.Sueepers = new SelectList(sueeperList, "Id", "Name");
            if (obj.PersonaLInfoId != null && obj.SueeperId != null)
            {

                Assignsueeper tbl = new Assignsueeper();
                tbl.PersonaLInfoId = Convert.ToInt32(obj.PersonaLInfoId);
                tbl.sueeperId = Convert.ToInt32(obj.SueeperId);
                tbl.JobStatus = "Incoming";
                tbl.Dateofservice = obj.dateofservice;
                tbl.Timeofservice = obj.timeofservice;
                tbl.journeystatus = "Pending";
                tbl.createDate = DateTime.Now;
                db.AssinSueeper.Add(tbl);
                db.SaveChanges();

                ViewBag.IsAssigned = 1;
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    serviceslist = (IOrderedQueryable<Getmodel>)serviceslist.Where(t => (t.FirstName + t.LastName + t.PersonName + t.ZipCode + t.Phone).ToLower().Contains(searchString.ToLower()));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 15;
            getModel = await PaginatedList<Getmodel>.CreateAsync(
                serviceslist.AsNoTracking(), pageIndex ?? 1, pageSize);

            getModel.CurrentFilter = searchString;
            return View(getModel);
        }
        [HttpGet]
        public IActionResult GetAssignedSueeper(int Id, string zipcode)
        {
            int? assignedsueeper = 0;
            var sueeperList = db.SueeperInfo.Where(p => p.Zipcode == zipcode).ToList();
            ViewBag.sueepers = sueeperList;
            var servicedetail = db.AssinSueeper.FirstOrDefault(m => m.PersonaLInfoId == Id);

            if (servicedetail != null && sueeperList != null)
            {
                assignedsueeper = servicedetail.sueeperId;
                sueeperList = sueeperList.OrderByDescending(m => m.Id == assignedsueeper).ToList();

            }
            List<SueeperViewModel> lstsueeper = new List<SueeperViewModel>();
            foreach (var item in sueeperList)
            {
                lstsueeper.Add(new SueeperViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    SelectedValue = servicedetail?.sueeperId

                });
            }

            return Json(lstsueeper);
        }



        [HttpGet]
        public IActionResult AssignSupeer(string serviceId, string serviceDate, string serviceTime, string sueeperId)
        {
            if (Convert.ToInt32(sueeperId) != 0)
            {

                var checklist = db.AssinSueeper.Where(m => m.PersonaLInfoId == Convert.ToInt32(serviceId)).FirstOrDefault();

                if (checklist == null)
                {
                    Assignsueeper tbl = new Assignsueeper();
                    tbl.PersonaLInfoId = Convert.ToInt32(serviceId);
                    tbl.sueeperId = Convert.ToInt32(sueeperId);
                    tbl.JobStatus = "Incoming";
                    tbl.Dateofservice = serviceDate;
                    tbl.Timeofservice = serviceTime;
                    tbl.journeystatus = "Pending";
                    tbl.createDate = DateTime.Now;
                    db.AssinSueeper.Add(tbl);
                    db.SaveChanges();

                    savesueeperid(serviceId, sueeperId);
                    ModelState.Clear();
                    return Json("Assigned Successfully");

                }
                ModelState.Clear();
                return Json("Already Assigned");
            }
            ModelState.Clear();
            return Json("Please select sueeper");
        }

        public void savesueeperid(string serviceid, string sueeperid)
        {
            var check = db.Servicestatus.Where(m => m.serviceid == Convert.ToInt32(serviceid)).FirstOrDefault();
            if (check != null)
            {
                check.SueeperId = Convert.ToInt32(sueeperid);
                check.Servicestatus = "Progress";
                db.SaveChanges();
            }
        }

        [HttpGet]

        public IActionResult Getsueeper(int id)
        {
            // var findtotal = "";
            var user = db.PersonalInfo.Where(m => m.Id == id).FirstOrDefault();
            var timesueep = db.TimeDateInfo.FirstOrDefault(m => m.PersonalInfoId == id);
            var userzip = user.ZipCode;


            var sueeperList = db.SueeperInfo.Where(p => p.Zipcode == userzip && p.IsBusy == "Availble").ToList();



            return Ok(sueeperList);

        }




        ////Assign  sueeper to service
        [HttpPost]
        public IActionResult AssignSueeper(AssignedSueeper obj)
        {

            return RedirectToAction("GetServices");
        }


        public IActionResult GetSueeperwithservices()
        {
            return View();
        }




    }
    public class AssignedSueeper
    {
        public string PersonaLInfoId { get; set; }
        public string SueeperId { get; set; }
        public string dateofservice { get; set; }
        public string timeofservice { get; set; }

    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sueep.Helpers;
using Sueep.Models;

namespace Sueep.Controllers
{

    public class SueeperController : Controller
    {
        private readonly Admincontext db;
        public PaginatedList<Sueeper> getModel { get; set; }
        public PaginatedList<StatusmodelClass> StatusmodelModel { get; set; }

        public SueeperController(Admincontext Db)
        {
            db = Db;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            var sueeperlist = (IQueryable<Sueeper>)db.SueeperInfo;

            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    sueeperlist = sueeperlist.Where(t => t.Name.ToLower().Contains(searchString.ToLower()));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 15;
            getModel = await PaginatedList<Sueeper>.CreateAsync(
                sueeperlist.AsNoTracking(), pageIndex ?? 1, pageSize);

            getModel.CurrentFilter = searchString;
            return View(getModel);
            //return View(sueeperlist);
        }
        [HttpGet]
        public IActionResult Createsueeper()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Createsueeper(SueeperModel model)

        {
            if (ModelState.IsValid)
            {
                try
                {
                    Sueeper tbl = new Sueeper();
                    var checktbl = db.SueeperInfo.Where(m => m.Id == tbl.Id).FirstOrDefault();
                    if (checktbl == null)
                    {
                        tbl.Name = model.Name;
                        tbl.Password = model.Password;
                        tbl.Phone = model.Phone;
                        tbl.Socialsecuritynumber = model.Socialsecuritynumber;
                        tbl.Zipcode = model.Zipcode;
                        tbl.IsBusy = "Availble";
                        tbl.Can_you_buy_cleaning_products = model.Can_you_buy_cleaning_products;
                        tbl.Do_you_have_car = model.Do_you_have_car;
                        tbl.Email = model.Email;
                        db.SueeperInfo.Add(tbl);
                        db.SaveChanges();
                        ModelState.Clear();
                        ViewBag.Message = "New Sueeper Created";
                        return View();
                    }
                    else
                    {
                        tbl.Name = model.Name;
                        tbl.Password = model.Password;
                        tbl.Phone = model.Phone;
                        tbl.Socialsecuritynumber = model.Socialsecuritynumber;
                        tbl.IsBusy = "Availble";
                        tbl.Can_you_buy_cleaning_products = model.Can_you_buy_cleaning_products;
                        tbl.Do_you_have_car = model.Do_you_have_car;
                        tbl.Email = model.Email;
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editdata = db.SueeperInfo.Where(m => m.Id == id).FirstOrDefault();
            return View(editdata);
        }
        [HttpPost]
        public IActionResult Edit(SueeperModel model)
        {
            Sueeper tble = new Sueeper();
            var editdata = db.SueeperInfo.Where(m => m.Id == model.id).FirstOrDefault();
            editdata.Name = model.Name;
            editdata.Phone = model.Phone;
            editdata.Socialsecuritynumber = model.Socialsecuritynumber;
            editdata.Zipcode = model.Zipcode;
            editdata.Do_you_have_car = model.Do_you_have_car;
            editdata.Email = model.Email;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult deleteSuueeper(int? id)
        {
            var checktbl = db.SueeperInfo.Where(m => m.Id == id).FirstOrDefault();
            if (checktbl != null)
            {
                db.SueeperInfo.Remove(checktbl);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View("Index");
        }

        public IActionResult sueeperdetail(int id)

        {
            var sueeper = (from a in db.SueeperInfo
                           join b in db.AssinSueeper on a.Id equals b.sueeperId
                           where a.Id == id
                           select new sueepdetail
                           {
                               Name = a.Name,
                               Email = a.Email,
                               Phone = a.Phone,
                               City = a.City,
                               Socialsecuritynumber = a.Socialsecuritynumber,
                               Zipcode = a.Zipcode,
                               Status = b.JobStatus,

                           }).FirstOrDefault();
            if (sueeper == null)
            {
                ViewBag.message = "Detail Not Availble";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ID = id;
                ViewBag.Name = sueeper.Name;
                ViewBag.Email = sueeper.Email;
                ViewBag.Phone = sueeper.Phone;
                ViewBag.City = sueeper.City;
                ViewBag.Socialsecuritynumber = sueeper.Socialsecuritynumber;
                ViewBag.Zipcode = sueeper.Zipcode;
            }
            return View();
        }
        //Service status here
        [HttpGet]
        public async Task<IActionResult> status(string sortOrder, string currentFilter, string searchString, int? pageIndex, string ServiceStatus)
        {
            var serviceList = (from details in db.PersonalInfo
                               join Add in db.AddressInfo on details.Id equals Add.PersonalInfoId
                               join TimeP in db.TimeDateInfo on details.Id equals TimeP.PersonalInfoId
                               join paytbl in db.PaymentTbl on details.Id equals paytbl.ServiceId
                               join assintbl in db.AssinSueeper on details.Id equals assintbl.PersonaLInfoId
                               join stsustable in db.Servicestatus on details.Id equals stsustable.serviceid


                               select new StatusmodelClass
                               {
                                   PesonalInfoId = details.Id,
                                   FirstName = details.FirstName,
                                   LastName = details.LastName,
                                   Servicestatus = stsustable.Servicestatus,
                                   Phone = details.Phone,
                                   ZipCode = details.ZipCode,
                                   dateofservice = TimeP.DateOfService,
                                   timeofservice = TimeP.TimeOfService,
                                   Email = details.Email,

                                   Status = assintbl.JobStatus,


                                   Amount = paytbl.PaymentAmount,
                                   // PersonName = sueep.Name,
                               });

            var rootpath = "http://sueep1.kindlebit.com";
            var GetImagelistData = (from c in db.AssinSueeper
                                    join b in db.SueeperImages on c.PersonaLInfoId equals b.ServiceID
                                    //where c.PersonaLInfoId == ServiceId
                                    select new GetImagelist
                                    {
                                        PictureId = b.PictureId,
                                        picturePath = b.picturePath,
                                        Imageurl = rootpath + b.Imageurl,
                                        SueeperId = b.SueeperId,
                                        Comment = b.Message,
                                        CreatedDate = b.img_date,
                                        ServiceID = b.ServiceID,
                                        P_Id = b.Pic_val
                                    }).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    serviceList = serviceList.Where(t => t.Status.ToLower().Contains(searchString.ToLower()) || t.dateofservice.ToLower().Contains(searchString.ToLower()) || t.FirstName.ToLower().Contains(searchString.ToLower()) || t.LastName.ToLower().Contains(searchString.ToLower()) || t.PersonName.ToLower().Contains(searchString.ToLower()) || t.Servicestatus.ToLower().Contains(searchString.ToLower()));
                }
                catch (Exception)
                {
                }
            }

            if (!string.IsNullOrEmpty(ServiceStatus))
            {

                serviceList = serviceList.Where(x => x.Servicestatus == ServiceStatus);
            }
            int pageSize = 15;
            try
            {

                StatusmodelModel = await PaginatedList<StatusmodelClass>.CreateAsync(
                    serviceList.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
            catch (Exception)
            {
                StatusmodelModel = new PaginatedList<StatusmodelClass>();
            }

            StatusmodelModel.CurrentFilter = searchString;
            StatusmodelModel.StatusImageTextList = GetImagelistData;

            return View(StatusmodelModel);
            //return View(serviceList.ToList());
        }
        [HttpGet]
        public IActionResult StatusEdit(int? id)
        {
            var cheklist = db.Servicestatus.Where(m => m.serviceid == id).FirstOrDefault();
            return View(cheklist);
            //try
            //{
            //    //if (!db.Servicestatus.Any())
            //{
            //    var cheklist = db.Servicestatus.Where(m => m.serviceid == id).FirstOrDefault();
            //    return View(cheklist);
            //}

            //    var serviceList = (from details in db.PersonalInfo
            //                       join Add in db.AddressInfo on details.Id equals Add.PersonalInfoId
            //                       join TimeP in db.TimeDateInfo on details.Id equals TimeP.PersonalInfoId
            //                       join paytbl in db.PaymentTbl on details.Id equals paytbl.ServiceId
            //                       join assintbl in db.AssinSueeper on details.Id equals assintbl.PersonaLInfoId
            //                       join stsustable in db.Servicestatus on details.Id equals stsustable.serviceid


            //                       select new StatusmodelClass
            //                       {
            //                           PesonalInfoId = Add.Id,
            //                           FirstName = details.FirstName,
            //                           LastName = details.LastName,
            //                           Servicestatus = stsustable.Servicestatus,
            //                           Phone = details.Phone,
            //                           ZipCode = details.ZipCode,
            //                           dateofservice = TimeP.DateOfService,
            //                           timeofservice = TimeP.TimeOfService,
            //                           Email = details.Email,

            //                           Status = assintbl.JobStatus,


            //                           Amount = paytbl.PaymentAmount,
            //                           // PersonName = sueep.Name,
            //                       });
            //    Servicestatusclass data = new Servicestatusclass();
            //    if (id.HasValue)
            //    {
            //        data.Servicestatus = serviceList.FirstOrDefault(x => x.PesonalInfoId == id)?.Servicestatus;
            //    }
            //    return View(data);
            //}
            //catch (Exception)
            //{
            //    return View();
            //}

        }
        [HttpPost]
        public IActionResult StatusEdit(StatusmodelClass model, string status)
        {
            try
            {
                var data = db.Servicestatus.FirstOrDefault(x => x.serviceid == model.Id);
                if (data != null)
                {
                    data.Servicestatus = model.Servicestatus;
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("status");
        }



        //public IActionResult StatusEdit(int? id)
        //{
        //    return View();
        //}
        public IActionResult Totalservices(int sueeperid)
        {
            var totalservices = (from a in db.SueeperInfo
                                 join b in db.AssinSueeper on a.Id equals b.sueeperId
                                 where a.Id == sueeperid
                                 select new totservices

                                 {

                                     DateofServices = b.Dateofservice,
                                     TimeofServices = b.Timeofservice,
                                     Status = b.JobStatus,
                                 }
                               ).ToList();
            return View(totalservices.ToList());
        }


        public class sueepdetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string City { get; set; }
            public string Socialsecuritynumber { get; set; }
            public Nullable<System.DateTime> createddate { get; set; }
            public string IsRole { get; set; }
            public string Do_you_have_car { get; set; }
            public string Can_you_buy_cleaning_products { get; set; }
            public string Zipcode { get; set; }
            public string Password { get; set; }
            public string lng { get; set; }
            public string lat { get; set; }
            public string IsBusy { get; set; }
        }


    }
}
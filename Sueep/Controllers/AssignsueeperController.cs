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

        public async Task<IActionResult> GetServices(AssignedSueeper obj, string sortOrder,
    string currentFilter, string searchString, int? pageIndex)
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
            var sueeperList = db.SueeperInfo.ToList();
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
                    serviceslist = (IOrderedQueryable<Getmodel>)serviceslist.Where(t => (t.FirstName + t.LastName + t.PersonName + t.ZipCode + t.Phone).Contains(searchString));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 5;
            getModel = await PaginatedList<Getmodel>.CreateAsync(
                serviceslist.AsNoTracking(), pageIndex ?? 1, pageSize);

            getModel.CurrentFilter = searchString;
            return View(getModel);

        }
        [HttpGet]
        public IActionResult GetAssignedSueeper(string Id)
        {
            var sueeperList = db.SueeperInfo.Where(p => p.Zipcode == Id).ToList();
            ViewBag.sueepers = sueeperList;
            return Json(sueeperList);
        }


        [HttpGet]
        public IActionResult AssignSupeer(string serviceId, string serviceDate, string serviceTime, string sueeperId)
        {
            if (Convert.ToInt32(sueeperId) != 0)
            {
                //var checksueeperandservices = db.AssinSueeper.Where(m => m.PersonaLInfoId == Convert.ToInt32(serviceId) &&u=> u.sueeperId == Convert.ToInt32(sueeperId)).FirstOrDefault();
                //if (checksueeperandservices != null)
                //{
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

            //var dateofservices =timesueep.DateOfService;
            //var converttime = Convert.ToDateTime(dateofservices);
            //if (converttime == DateTime.Today)
            //{ }
            var sueeperList = db.SueeperInfo.Where(p => p.Zipcode == userzip && p.IsBusy == "Availble").ToList();

            //foreach (var data in sueeperList)
            //{
            //    var assigntble = db.AssinSueepers.Where(m => m.sueeperId == data.Id).FirstOrDefault();
            //    var dateofservices = assigntble.Dateofservice;
            //    var converttime = Convert.ToDateTime(dateofservices);
            //    if (converttime == DateTime.Today)
            //    {

            //    }

            //}

            return Ok(sueeperList);

        }


        //[HttpGet]
        //[Route("GetServices")]
        //public IHttpActionResult GetServices()
        //{

        //    var serviceList = (from details in db.PersonalInfoes
        //                       join Add in db.AddressInfoes on details.Id equals Add.PersonalInfoId
        //                       join TimeP in db.TimeDateInfoes on details.Id equals TimeP.PersonalInfoId
        //                       join paytbl in db.PaymentTbls on details.Id equals paytbl.ServiceId

        //                       select new GetModel
        //                       {
        //                           Id = details.Id,
        //                           FirstName = details.FirstName,
        //                           LastName = details.LastName,
        //                           // NoOfBathrooms = details.NoOfBathrooms,
        //                           //  NoOfBedrooms = details.NoOfBedrooms,
        //                           Phone = details.Phone,
        //                           ZipCode = details.ZipCode,
        //                           dateofservice = TimeP.DateOfService,
        //                           timeofservice = TimeP.TimeOfService,
        //                           Email = details.Email,
        //                           Status = paytbl.JobStatus,

        //                           //details.CreatedDate,
        //                           //Add.Address,
        //                           //Add.State,
        //                           //TimeP.ExtraService,
        //                           //TimeP.TimeOfService,
        //                           //TimeP.DateOfService

        //                       }).ToList().OrderByDescending(m => m.Id);
        //    return Ok(serviceList);

        //}

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

        //public void sueeperstatus(int sueeperid)
        //{
        //    var checkseeperlist = db.SueeperInfoes.FirstOrDefault(m => m.Id == sueeperid);
        //    if (checkseeperlist != null)
        //    {
        //        checkseeperlist.IsBusy = "Busy";
        //        db.SaveChanges();
        //    }
        //}

        //public IHttpActionResult changestatus(int? id, string status)
        //{
        //    var checklist = db.PaymentTbls.FirstOrDefault(m => m.ServiceId == id);
        //    if (checklist != null)
        //    {
        //        checklist.JobStatus = status;
        //        db.SaveChanges();
        //    }
        //    return Ok();
        //}
        //public void SendEmailsueeper(PersonalInfo app, int id)
        //{
        //    // string Smtp_UserEmail = ConfigurationManager.AppSettings[""];
        //    // string Smtp_Userpassword = ConfigurationManager.AppSettings[""];

        //    var sueeperInfo = db.SueeperInfoes.Where(p => p.Id == id).FirstOrDefault();
        //    string html = "";
        //    html = "<html><body><p><strong>Name :-</strong>" + app.FirstName + "</p><p><strong>Phone :-</strong>" + app.Phone + "</p></body></html>";
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient();
        //        MailMessage mail = new MailMessage();
        //        SmtpServer.UseDefaultCredentials = true;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
        //        SmtpServer.Port = 587;
        //        SmtpServer.Host = "smtp.gmail.com";
        //        SmtpServer.EnableSsl = true;
        //        mail = new MailMessage();
        //        mail.From = new MailAddress("");
        //        mail.To.Add(new MailAddress(sueeperInfo.Email));
        //        mail.Subject = "New Job";
        //        mail.IsBodyHtml = true;
        //        mail.Body = html;
        //        SmtpServer.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        ////send email
        //public void SendEmailUSer(SueeperInfo app, string email)
        //{

        //    string html = "";
        //    html = "<html><body><p><strong>Name :-</strong>" + app.Name + "</p><p><strong>Phone :-</strong>" + app.Phone + "</p><p><strong>Name :-</strong>" + app.Name + "</p><p><p><strong>Name :-</strong>" + app.Name + "</p></body></html>";
        //    try
        //    {
        //        SmtpClient SmtpServer = new SmtpClient();
        //        MailMessage mail = new MailMessage();
        //        SmtpServer.UseDefaultCredentials = true;
        //        SmtpServer.Credentials = new System.Net.NetworkCredential("", "");
        //        SmtpServer.Port = 587;
        //        SmtpServer.Host = "smtp.gmail.com";
        //        SmtpServer.EnableSsl = true;
        //        mail = new MailMessage();
        //        mail.From = new MailAddress("");
        //        mail.To.Add(new MailAddress(email));
        //        mail.Subject = "Sueeper Detail";
        //        mail.IsBodyHtml = true;
        //        mail.Body = html;
        //        SmtpServer.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        ////Get sueeper list Match with services postalcode....

        //[HttpGet]
        //[Route("Getsueeper")]
        //public IHttpActionResult Getsueeper(int id)
        //{
        //    // var findtotal = "";
        //    var user = db.PersonalInfoes.Where(m => m.Id == id).FirstOrDefault();
        //    var timesueep = db.TimeDateInfoes.FirstOrDefault(m => m.PersonalInfoId == id);
        //    var userzip = user.ZipCode;

        //    //var dateofservices =timesueep.DateOfService;
        //    //var converttime = Convert.ToDateTime(dateofservices);
        //    //if (converttime == DateTime.Today)
        //    //{ }
        //    var sueeperList = db.SueeperInfoes.Where(p => p.Zipcode == userzip && p.IsBusy == "Availble").ToList();

        //    //foreach (var data in sueeperList)
        //    //{
        //    //    var assigntble = db.AssinSueepers.Where(m => m.sueeperId == data.Id).FirstOrDefault();
        //    //    var dateofservices = assigntble.Dateofservice;
        //    //    var converttime = Convert.ToDateTime(dateofservices);
        //    //    if (converttime == DateTime.Today)
        //    //    {

        //    //    }

        //    //}

        //    return Ok(sueeperList);

        //}
    }
    public class AssignedSueeper
    {
        public string PersonaLInfoId { get; set; }
        public string SueeperId { get; set; }
        public string dateofservice { get; set; }
        public string timeofservice { get; set; }

    }


}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sueep.Helpers;
using Sueep.Models;

namespace Sueep.Controllers
{
    public class ServiceController : Controller
    {
        private readonly Admincontext db;
        public PaginatedList<Getmodel> getModel { get; set; }
        public PaginatedList<Sueep.Models.Users> UsersModel { get; set; }

        public ServiceController(Admincontext Db)
        {
            db = Db;
        }

        [Authorize]
        public async Task<IActionResult> Services(string search, string jobstatus, DateTime? firstdate, DateTime? enddate, string sortOrder, string currentFilter, string searchString, int? pageIndex)

        {
            //if (TempData["name"] == null)
            //{
            //    return View("Adminlogin");
            //}
            //else
            //{
            List<Assignsueeper> AssinSueeper = db.AssinSueeper.ToList();
            List<Sueeper> SueeperInfo = db.SueeperInfo.ToList();


            var serviceList = (from details in db.PersonalInfo
                               join Add in db.AddressInfo on details.Id equals Add.PersonalInfoId
                               join TimeP in db.TimeDateInfo on details.Id equals TimeP.PersonalInfoId
                               join paytbl in db.PaymentTbl on details.Id equals paytbl.ServiceId
                               join assintbl in AssinSueeper on details.Id equals assintbl.PersonaLInfoId
                               join statustable in db.Servicestatus on details.Id equals statustable.serviceid
                               join sueep in db.SueeperInfo on assintbl.sueeperId equals sueep.Id

                               select new Getmodel
                               {
                                   FirstName = details.FirstName,
                                   LastName = details.LastName,

                                   Phone = details.Phone,
                                   ZipCode = details.ZipCode,
                                   dateofservice = TimeP.DateOfService,
                                   timeofservice = TimeP.TimeOfService,
                                   Email = details.Email,
                                   Status = statustable.Servicestatus,


                                   Amount = paytbl.PaymentAmount,
                                   PersonName = sueep.Name,
                               });
            if (jobstatus != null)
            {
                if (jobstatus == "All")
                {
                    //serviceList = serviceList;
                }
                else
                {
                    serviceList = serviceList.Where(m => m.Status == jobstatus);
                }
            }
            if (firstdate != null && enddate != null)
            {
                serviceList = serviceList.Where(x => Convert.ToDateTime(x.dateofservice) >= firstdate && Convert.ToDateTime(x.dateofservice) <= enddate);
            }
            //if (!String.IsNullOrEmpty(firstdate))
            //{
            //    serviceList = serviceList.Where(m => m.dateofservice ==  firstdate).ToList();
            //}
            if (!String.IsNullOrEmpty(search))
            {
                serviceList = serviceList.Where(m => m.PersonName.Contains(search));
            }


            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    serviceList = (IOrderedQueryable<Getmodel>)serviceList.Where(t => (t.FirstName + t.LastName + t.PersonName + t.Status + t.Phone + t.dateofservice + t.Amount).ToLower().Contains(searchString.ToLower()));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 15;
            getModel = await PaginatedList<Getmodel>.CreateAsync(
                serviceList.AsNoTracking(), pageIndex ?? 1, pageSize);

            getModel.CurrentFilter = searchString;
            return View(getModel);

            //return View(serviceList);

        }
        [Authorize]
        public IActionResult CreateServices()
        {
            return View();
        }


        public IActionResult Adminlogin()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Adminlogin(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var check = db.UserRegistration.Where(m => m.Email == model.Email && m.Password == model.password && m.IsRole == "Admin").FirstOrDefault();
                if (check != null)
                {
                    HttpContext.Session.SetString("Email", check.Email);
                    var claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Name, check.Email));

                    //string[] roles = check.IsRole.Split(",");

                    //foreach (string role in roles)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, role));
                    //}

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var props = new AuthenticationProperties();
                    props.IsPersistent = true;
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                    return RedirectToAction("Dashboard", "Service", new { area = "" });

                }
                else
                {
                    model.LoginError = "Email or Password is incorrect";

                    return View("Adminlogin", model);
                }
            }
            return View();
        }
        [Authorize]
        public IActionResult Dashboard(string search)
        {
            DashBoardModel model = new DashBoardModel();
            var pendingsstatus = db.Servicestatus.Where(m => m.Servicestatus == "Pending").Count();
            // var pendings= statusdata.
            var Progresssstatus = db.Servicestatus.Where(m => m.Servicestatus == "In Progress").Count();
            var Completestatus = db.Servicestatus.Where(m => m.Servicestatus == "Complete").Count();

            // var listdata=(from a in db.AssinSueeper

            //var lastsevendaysdata =db.AssinSueeper.orderbyde
            //if(string.IsNullOrEmpty(HttpContext.Session.GetStrn ing("Email")))
            //     {
            //         return RedirectToAction("Adminlogin");
            //     }


            ViewBag.Pendings = pendingsstatus;
            ViewBag.Progress = Progresssstatus;
            ViewBag.Complete = Completestatus;


            //table data for dashboard

            var serviceList = (from details in db.PersonalInfo
                               join Add in db.AddressInfo on details.Id equals Add.PersonalInfoId
                               join TimeP in db.TimeDateInfo on details.Id equals TimeP.PersonalInfoId
                               join paytbl in db.PaymentTbl on details.Id equals paytbl.ServiceId
                               join assintbl in db.AssinSueeper on details.Id equals assintbl.PersonaLInfoId
                               join stsustable in db.Servicestatus on assintbl.PersonaLInfoId equals stsustable.serviceid

                               select new Getmodel
                               {
                                   FirstName = details.FirstName,
                                   LastName = details.LastName,

                                   Phone = details.Phone,
                                   ZipCode = details.ZipCode,
                                   dateofservice = TimeP.DateOfService,
                                   timeofservice = TimeP.TimeOfService,
                                   Email = details.Email,

                                   // PaymentStatus = "Paid",
                                   Status = stsustable.Servicestatus,


                                   Amount = paytbl.PaymentAmount,

                               }).OrderByDescending(m => m.dateofservice).ToList();

            DateTime today = DateTime.Now;
            DateTime seventhday = DateTime.Now.Date.AddDays(-7);
            // seventhday = seventhday.AddDays(-7);    
            serviceList = serviceList.Where(x => Convert.ToDateTime(x.dateofservice) <= today && Convert.ToDateTime(x.dateofservice) <= seventhday).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                string temp = search.ToLower();
                model.SearchString = search;
                serviceList = serviceList.Where(x => x.FirstName.ToLower().Contains(temp) || x.LastName.ToLower().Contains(temp) || x.Email.ToLower().Contains(temp) || x.Phone.ToLower().Contains(temp) || x.ZipCode.ToLower().Contains(temp) || x.Amount.ToLower().Contains(temp) || x.Status.ToLower().Contains(temp))?.ToList();
            }
            model.GetModel = serviceList;
            return View(model);
        }


        public IActionResult statustracking(string Status, int id)
        {
            if (Status == "Pending")
            {
                var pendingdata = (from a in db.PersonalInfo
                                   join b in db.AddressInfo on a.Id equals b.PersonalInfoId
                                   join c in db.TimeDateInfo on a.Id equals c.PersonalInfoId
                                   join d in db.PaymentTbl on a.Id equals d.ServiceId
                                   where a.Id == id
                                   select new Getmodel
                                   {
                                       FirstName = a.FirstName,
                                       timeofservice = c.TimeOfService,
                                       dateofservice = c.DateOfService,
                                       Email = a.Email,
                                       Amount = d.PaymentAmount

                                   }).FirstOrDefault();

                return View(pendingdata);

            }
            else if (Status == "In Progress")
            {
                var pendingdata = (from a in db.PersonalInfo
                                   join b in db.AddressInfo on a.Id equals b.PersonalInfoId
                                   join c in db.TimeDateInfo on a.Id equals c.PersonalInfoId
                                   join d in db.PaymentTbl on a.Id equals d.ServiceId
                                   join e in db.AssinSueeper on a.Id equals e.PersonaLInfoId
                                   join f in db.SueeperInfo on e.sueeperId equals f.Id

                                   where a.Id == id
                                   select new Getmodel
                                   {
                                       FirstName = a.FirstName,
                                       timeofservice = c.TimeOfService,
                                       dateofservice = c.DateOfService,
                                       Email = a.Email,
                                       Amount = d.PaymentAmount,
                                       PersonName = f.Name


                                   }).FirstOrDefault();
                return View(pendingdata);
            }
            else if (Status == "Complete")
            {
                GetImagelist items = new GetImagelist();

                //  Uri baseUri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, String.Empty));
                var x = "http://sueep1.kindlebit.com";
                var a = (from c in db.AssinSueeper

                         join b in db.SueeperImages on c.PersonaLInfoId equals b.ServiceID
                         //join d in db.Messagetbls on c.PersonaLInfoId equals d.serviceid

                         where c.PersonaLInfoId == id
                         select new GetImagelist
                         {
                             PictureId = b.PictureId,
                             picturePath = b.picturePath,
                             Imageurl = x + b.Imageurl,
                             SueeperId = b.SueeperId,

                             Comment = b.Message,
                             CreatedDate = b.img_date,
                             ServiceID = id,
                             P_Id = b.Pic_val
                         }).ToList();
                return View(a);
            }
            else
            {

            }
            //   var data = db.SueeperImages.Where(m => m.PictureId == PictureId).FirstOrDefault();
            // var datas = ata.Imageurl;
            //  var imagereturn = "http://sueep1.kindlebit.com" + datas;
            return View();
        }

        public async Task<IActionResult> Pastservices(int? ServiceId = 210)
        {
            GetImagelist items = new GetImagelist();
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
                            ServiceID = ServiceId,
                            P_Id = b.Pic_val
                        }).ToList();

            return View(GetImagelistData);

        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Adminlogin");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cutomers(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            //var customerslist = db.UserRegistration.ToList();
            var customerslist = (IQueryable<Users>)db.UserRegistration;

            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    customerslist = customerslist.Where(t => t.PersonName.ToLower().Contains(searchString.ToLower()) || t.City.ToLower().Contains(searchString.ToLower()) || t.Email.ToLower().Contains(searchString) || t.Gender.ToLower().Contains(searchString.ToLower()) || t.PhoneNumber.ToLower().Contains(searchString.ToLower()) || t.Country.ToLower().Contains(searchString.ToLower()));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 15;
            UsersModel = await PaginatedList<Users>.CreateAsync(
                customerslist.AsNoTracking(), pageIndex ?? 1, pageSize);

            UsersModel.CurrentFilter = searchString;
            return View(UsersModel);

            //return View(customerslist);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var editdata = db.UserRegistration.Where(m => m.RegisterId == id).FirstOrDefault();
            return View(editdata);

            // return View(editdata);
        }

        public IActionResult Customerservices(string Email = null)
        {
            var checklist = (from a in db.UserRegistration
                             join b in db.PersonalInfo on a.Email equals b.Email
                             join c in db.AddressInfo on b.Id equals c.PersonalInfoId
                             join d in db.TimeDateInfo on c.PersonalInfoId equals d.PersonalInfoId
                             join e in db.PaymentTbl on d.PersonalInfoId equals e.ServiceId
                             where a.Email == Email
                             select new Getmodel
                             {
                                 dateofservice = d.DateOfService,
                                 timeofservice = d.TimeOfService,
                                 Amount = e.PaymentAmount,
                                 Status = e.JobStatus,
                             }).ToList();
            return View(checklist);
        }

        //public IActionResult Edit(int id)
        //{
        //    var editdata = db.UserRegistration.Where(m => m.RegisterId == id).FirstOrDefault();
        //    return View(editdata);
        //}

        //  
        // POST: /CRUD/Edit/5  

        [HttpPost]

        public ActionResult Edit(Users employee, int id)
        {
            var checkdata = db.UserRegistration.Where(m => m.RegisterId == id).FirstOrDefault();
            if (checkdata != null)
            {
                checkdata.PersonName = employee.PersonName;
                checkdata.Email = employee.Email;
                checkdata.PhoneNumber = employee.PhoneNumber;
                checkdata.City = employee.City;
                checkdata.Country = employee.Country;
                db.SaveChanges();
                return RedirectToAction("Cutomers");

            }
            //if (ModelState.IsValid)
            //{
            //    //db.Entry(employee).State = EntityState.Modified;
            //    db.Entry(employee).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Cutomers");
            //}
            return View(employee);
        }
        public IActionResult deleteservices(int? id)
        {
            var checklist = db.UserRegistration.Where(m => m.RegisterId == id).FirstOrDefault();
            if (checklist != null)
            {
                db.UserRegistration.Remove(checklist);
                db.SaveChanges();
            }
            return RedirectToAction("Cutomers");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSueeperList()
        {
            var sueeperList = db.SueeperInfo.ToList();
            return Json(new SelectList(sueeperList, "Id", "Name"));
        }

    }
}
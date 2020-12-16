using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                                   Status = assintbl.JobStatus,


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
                    serviceList = (IOrderedQueryable<Getmodel>)serviceList.Where(t => (t.FirstName + t.LastName + t.PersonName + t.Status + t.Phone + t.dateofservice + t.Amount).Contains(searchString));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 5;
            getModel = await PaginatedList<Getmodel>.CreateAsync(
                serviceList.AsNoTracking(), pageIndex ?? 1, pageSize);

            getModel.CurrentFilter = searchString;
            return View(getModel);

            //return View(serviceList);

        }

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
                var check = db.UserRegistration.Where(m => m.Email == model.Email && m.Password == model.password).FirstOrDefault();
                if (check != null)
                {
                    HttpContext.Session.SetString("Email", check.Email);
                    //TempData["name"] = "check"; 
                    //return RedirectToAction()
                    // Session["VariableName"] = check.RegisterId;

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
        public IActionResult Dashboard()
        {
            //if(string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
            //     {
            //         return RedirectToAction("Adminlogin");
            //     }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Adminlogin");
        }
        [HttpGet]
        public async Task<IActionResult> Cutomers(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            //var customerslist = db.UserRegistration.ToList();
            var customerslist = (IQueryable<Users>)db.UserRegistration;

            if (!string.IsNullOrEmpty(searchString))
            {
                try
                {
                    customerslist = customerslist.Where(t => t.PersonName.Contains(searchString) || t.City.Contains(searchString) || t.Email.Contains(searchString) || t.Gender.Contains(searchString) || t.PhoneNumber.Contains(searchString)|| t.Country.Contains(searchString));
                }
                catch (Exception)
                {
                }
            }
            int pageSize = 5;
            UsersModel = await PaginatedList<Users>.CreateAsync(
                customerslist.AsNoTracking(), pageIndex ?? 1, pageSize);

            UsersModel.CurrentFilter = searchString;
            return View(UsersModel);

            //return View(customerslist);
        }


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


    }
}
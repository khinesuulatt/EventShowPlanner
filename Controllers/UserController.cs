using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventShow.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class UserController : Controller
    {
        private readonly EventShowPlannerContext _db;

        public UserController(EventShowPlannerContext db)
        {
            _db = db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {

            List<User> users = new List<User>();
            using (var db = new EventShowPlannerContext())
            {
                var usr = (from u in db.Users

                           select new
                           {
                               u.UserId,
                               u.UserName,
                               u.UserAge,
                               u.UserAddress,
                               u.Gender,
                               u.CityId,
                               u.UcreatedDate,
                               u.JobTitle,
                               u.Password,
                               u.PhoneNumber,
                               u.EmailAddress,
                               u.EventCount,
                               u.Active
                           }).ToList();
                foreach (var r in usr)
                {
                    users.Add(new Models.User
                    {
                        UserId = r.UserId,
                        UserName = r.UserName,
                        UserAge = r.UserAge,
                        UserAddress = r.UserAddress,
                        Gender = r.Gender,
                        CityId = r.CityId,
                        UcreatedDate = r.UcreatedDate,
                        JobTitle = r.JobTitle,
                        EventCount = r.EventCount,
                        Password = r.Password,
                        PhoneNumber = r.PhoneNumber,
                        EmailAddress = r.EmailAddress,
                        Active = r.Active,
                    });
                }
            }
            return View(users);
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            List<City> cities = new List<City>();
            cities = (from city in _db.Cities select city).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            User u = new User();
            using (var db = new EventShowPlannerContext())
            {
                var ur = (from z in db.Users
                          join c in db.Cities
                          on z.CityId equals c.CityId
                          where z.UserId == id
                          select new
                          {
                              z.UserId,
                              z.UserName,
                              z.UserAge,
                              z.UserAddress,
                              z.Gender,
                              z.EventCount,
                              z.CityId,
                              z.Active,
                              z.JobTitle,
                              z.EmailAddress,
                              z.PhoneNumber,
                              z.Password,
                              z.UcreatedDate,
                              c.CityName,
                          }).FirstOrDefault();

                u.UserId = ur.UserId;
                u.UserName = ur.UserName;
                u.UserAge = ur.UserAge;
                u.UserAddress = ur.UserAddress;
                u.UcreatedDate = ur.UcreatedDate;
                u.JobTitle = ur.JobTitle;
                u.Gender = ur.Gender;
                u.EventCount = ur.EventCount;
                u.Password = ur.Password;
                u.PhoneNumber = ur.PhoneNumber;
                u.EmailAddress = ur.EmailAddress;
                u.CityId = ur.CityId;
                u.Active = ur.Active;

                return View(u);
            }
        }

       
        public IActionResult Lock(int id)
        {
            using (var db = new EventShowPlannerContext())
            {

                var ur = (from z in db.Users
                          where z.UserId == id
                          select z).FirstOrDefault();
                
                ur.Active = false;
                

                db.Entry(ur).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute(new { controller = "User", action = "Index" });
            }  

        }

        public IActionResult Unlock(int id)
        {
            using (var db = new EventShowPlannerContext())
            {

                var ur = (from z in db.Users
                          where z.UserId == id
                          select z).FirstOrDefault();

                ur.Active = true;


                db.Entry(ur).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute(new { controller = "User", action = "Index" });
            }

        }

    }
}
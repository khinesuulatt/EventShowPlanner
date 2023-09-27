using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventShow.Models;
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
    public class CityController : Controller
    {
        private readonly EventShowPlannerContext _db;

            public CityController(EventShowPlannerContext db)
        {
            _db = db;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            List<City> cities = new List<City>();
            using (var db = new EventShowPlannerContext())
            {
                var result = (from c in db.Cities
                              where c.Active == true 
                              select new
                              {
                                  c.CityId,
                                  c.CityName,
                                  c.Active,
                                  c.CcreatedDate
                              }).ToList();

                foreach (var t in result)
                {
                    cities.Add(new City
                    {

                        CityId = t.CityId,
                        CityName = t.CityName,
                        Active = t.Active,
                        CcreatedDate = t.CcreatedDate
                    });
                }

                return View(cities);

            }

        }

        [HttpGet]
        public IActionResult Add()
        {
           
            return View();
        }


        [HttpPost]
        public IActionResult Add(City newcity)

        {
            using (var db = new EventShowPlannerContext())
            {
                var result = (from f in db.Cities
                              where f.CityName == newcity.CityName &&
                              f.Active == true
                              select f).FirstOrDefault();

                if (result != null)
                {
                    ViewData["result"] = "1";
                    return View(newcity);
                }
             
            }

            City city = new City()
            {
                
                CityName = newcity.CityName,
                Active = true,
                CcreatedDate = DateTime.Now,
            };

            _db.Cities.Add(city);
            _db.Entry(city).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return RedirectToRoute(new { controller = "City", action = "Index" });

        }

        public IActionResult Edit(int id)
        {
            City cedit = new City();
            using (var db = new EventShowPlannerContext())
            {
                var result1 = (from e in db.Cities
                              where e.CityId == id 
                              
                              select new
                              {
                                  e.CityId,
                                  e.CityName,
                                  e.Active
                                  
                              }).FirstOrDefault();

                cedit.CityId = result1.CityId;
                cedit.CityName = result1.CityName;
                cedit.Active = result1.Active;
                

                return View(cedit);
            }
               
        }

        [HttpPost]
        public IActionResult Edit(City city)
        {
            using (var db = new EventShowPlannerContext())
            {
                var cityinfo = (from t in db.Cities
                                where t.CityId == city.CityId 
                                
                                select t).FirstOrDefault();

                cityinfo.CityName = city.CityName;
                cityinfo.Active = city.Active;

                db.Entry(cityinfo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToRoute(new { controller = "City", action = "Index" });
        }


    }

    
}


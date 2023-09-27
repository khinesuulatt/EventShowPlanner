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
    public class EventCategoryController : Controller
    {
        private readonly EventShowPlannerContext _db;

        public EventCategoryController(EventShowPlannerContext db)
        {
            _db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<EventCategory> eventCategories = new List<EventCategory>();
            using (var db = new EventShowPlannerContext())
            {
                var result = (from e in db.EventCategories

                              select new
                              {
                                  e.EventCategoryId,
                                  e.EcategoryName,
                                  e.EccreatedDate,
                                  e.UpdateDate
                              }).ToList();

                foreach (var e in result)
                {
                    eventCategories.Add(new EventCategory
                    {

                        EventCategoryId = e.EventCategoryId,
                        EcategoryName = e.EcategoryName,
                        EccreatedDate = e.EccreatedDate,
                        UpdateDate = e.UpdateDate
                    });
                }

                return View(eventCategories);

            }
        }


        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Add(EventCategory eventctgo)
        {
            using (var db = new EventShowPlannerContext())
            {
                var result = (from f in db.EventCategories
                              where f.EcategoryName == eventctgo.EcategoryName
                              select f).FirstOrDefault();

                if (result != null)
                {
                    ViewData["result"] = "1";
                    return View(eventctgo);
                }


            }

            EventCategory ecty = new EventCategory()
            {
                EventCategoryId = eventctgo.EventCategoryId,
                EcategoryName = eventctgo.EcategoryName,
                EccreatedDate = DateTime.Now,
                
            };
            

            _db.EventCategories.Add(ecty);
            _db.Entry(ecty).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();


            return RedirectToRoute(new { controller = "EventCategory", action = "Index" });

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            EventCategory gory = new EventCategory();
            using ( var db = new EventShowPlannerContext())
            {
                var result = (from c in db.EventCategories
                              where c.EventCategoryId == id
                              select new
                              {
                                  c.EventCategoryId,
                                  c.EcategoryName,
                                  c.EccreatedDate,
                                  c.UpdateDate

                              }).FirstOrDefault();

                gory.EventCategoryId = result.EventCategoryId;
                gory.EcategoryName = result.EcategoryName;
                gory.UpdateDate = DateTime.Now;

                return View(gory);
            }
        }

        [HttpPost]
        public IActionResult Edit(EventCategory ecry)
        {
            using (var db = new EventShowPlannerContext())
            {
                var evct = (from e in db.EventCategories
                            where e.EventCategoryId == ecry.EventCategoryId
                            select e).FirstOrDefault();
                evct.EventCategoryId = ecry.EventCategoryId;
                evct.EcategoryName = ecry.EcategoryName;
                evct.UpdateDate = DateTime.Now;

                db.Entry(evct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToRoute(new { controller = "EventCategory", action = "Index" });
        }

        //[HttpPost]
        //public IActionResult Delete(EventCategory xyz)
        //{
        //    using (var db = new EventShowPlannerContext())
        //    {
        //        var ez = (from a in db.EventCategories
        //                  where a.EventCategoryId == xyz.EventCategoryId
        //                  select a).FirstOrDefault();

        //        db.Entry(ez).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        //        db.SaveChanges();

                

        //        return RedirectToRoute(new { controller = "EventCategory", action = "Index" });

        //    }
        //}
    }
}


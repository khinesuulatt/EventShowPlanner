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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class AdminController : Controller
    {
        private readonly EventShowPlannerContext _db;

        public AdminController(EventShowPlannerContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(Admin ad)
        {
            List<Admin> admins = new List<Admin>();
            int? admin_id = 0;
            string? admin_name = "";

            using (var db = new EventShowPlannerContext())
            {

                var result = (from d in db.Admins

                              select new
                              {
                                  d.AdminId,
                                  d.AdminName,
                                  d.AdminPassword,
                                  d.Active

                              }).ToList();


                var admin = (from a in _db.Admins
                             where a.AdminName == ad.AdminName &&
                             a.AdminPassword == ad.AdminPassword &&
                             a.Active == true
                             select a).FirstOrDefault();

                if(admin != null)
                {
                    admin_id = admin.AdminId;
                    admin_name = admin.AdminName;
                }
                

                if (admin != null)
                {
                    return Json(new { success = true ,id = admin_id, name = admin_name});
                    
                }

                else
                {
                    return Json(new { success = false ,id = 0, name = ""});
                }

            }
        }
    }
}


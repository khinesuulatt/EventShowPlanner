using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventShow.Models;
using System.Data.SqlClient;
using System.Collections;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Web;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.IO;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Server;
using Microsoft.Extensions.Hosting;
using System.Reflection;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly EventShowPlannerContext _db;
        private readonly IWebHostEnvironment _he;

        public object Server { get; private set; }

        public SpeakerController(EventShowPlannerContext db, IWebHostEnvironment he)
        {
            _he = he;
            _db = db;
        }

        public IActionResult Index()
        {
            List<Speakerinfoindex> speakers = new List<Speakerinfoindex>();

            using (var db = new EventShowPlannerContext())
            {
                var result = (from s in db.Speakers
                              join e in db.Cities
                              on s.CityId equals e.CityId
                              select new
                              {
                                  s.SpeakerId,
                                  s.SpeakerName,
                                  s.Sage,
                                  s.Professional,
                                  s.Sjob,
                                  s.Description,
                                  s.Saddress,
                                  s.CityId,
                                  e.CityName,
                                  s.ScreatedDate,
                                  s.SpeakerImgPath,
                                  s.Active
                              }).ToList();

                foreach (var p in result)
                {
                    speakers.Add(new Speakerinfoindex
                    {
                        SpeakerId = p.SpeakerId,
                        SpeakerName = p.SpeakerName,
                        Sage = p.Sage,
                        Professional = p.Professional,
                        Sjob = p.Sjob,
                        Description = p.Description,
                        Saddress = p.Saddress,
                        CityId = p.CityId,
                        ScreatedDate = p.ScreatedDate,
                        CityName = p.CityName,
                        SpeakerImgPath = p.SpeakerImgPath,
                        Active = p.Active
                    });

                    
                }

                return View(speakers);
            }

        }

        [HttpGet]
        public IActionResult AddNewSpeaker()
        {
            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            return View();
        }

        [HttpPost]
        public IActionResult AddNewSpeaker(Speakerinfo newspeaker)
        {
            string FilePath = "";
            string RelativePath = "";
            if (newspeaker.SpeakerImgPath != null)
            {

                FilePath = Path.Combine(
                    new string[]
                    {
                _he.WebRootPath, "img" , newspeaker.SpeakerImgPath.FileName
                    });

                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    newspeaker.SpeakerImgPath.CopyTo(fs);

                }

                RelativePath = Path.Combine(
                    new string[]
                    {
                 "img" , newspeaker.SpeakerImgPath.FileName
                    });
            }

            using (var db = new EventShowPlannerContext())
            {
                var result = (from r in db.Speakers
                              where r.SpeakerName == newspeaker.SpeakerName &&
                              r.Active == true
                              select r).FirstOrDefault();

                if (result != null)
                {
                    ViewData["result"] = "1";
                    return View(newspeaker);
                }


            }

            Speaker spk = new Speaker()

            {
                //SpeakerId = newspeaker.SpeakerId,
                SpeakerName = newspeaker.SpeakerName,
                Sage = newspeaker.Sage,
                Professional = newspeaker.Professional,
                Sjob = newspeaker.Sjob,
                Description = newspeaker.Description,
                Saddress = newspeaker.Saddress,
                CityId = newspeaker.CityId,
                ScreatedDate = DateTime.Now,
                Active = newspeaker.Active,

                SpeakerImgPath = RelativePath,
            };

            _db.Speakers.Add(spk);
            _db.Entry(spk).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

            return RedirectToRoute(new { controller = "Speaker", action = "Index" });


        }


        [HttpGet]

        public IActionResult Edit(int id)
        {
            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;


            Speakerinfoindex sp = new Speakerinfoindex();

            using (var db = new EventShowPlannerContext())
            {
                var result = (from p in db.Speakers
                              join e in db.Cities
                              on p.CityId equals e.CityId
                              where p.SpeakerId == id
                              select new
                              {
                                  p.SpeakerId,
                                  p.SpeakerName,
                                  p.Sage,
                                  p.Professional,
                                  p.Sjob,
                                  p.Description,
                                  p.Saddress,                               
                                  p.ScreatedDate,
                                  p.SpeakerImgPath,
                                  p.Active,
                                  e.CityId,
                                  e.CityName,

                              }).FirstOrDefault();

                sp.SpeakerId = result.SpeakerId;
                sp.SpeakerName = result.SpeakerName;
                sp.Sage = result.Sage;
                sp.Professional = result.Professional;
                sp.Sjob = result.Sjob;
                sp.Description = result.Description;
                sp.Saddress = result.Saddress;
                sp.CityId = result.CityId;
                sp.CityName = result.CityName;
                sp.ScreatedDate = result.ScreatedDate;

                sp.SpeakerImgPath = result.SpeakerImgPath;

                sp.Active = result.Active; 

                return View(sp);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Speakerinfo spk, IFormFile photo)
        {
            string FilePath = "";
            string RelativePath = "";
            if (spk.SpeakerImgPath != null)
            {
                FilePath = Path.Combine(
                    new string[]
                    {
                _he.WebRootPath, "img" , spk.SpeakerImgPath.FileName
                    });

                RelativePath = Path.Combine(
                   new string[]
                   {
                 "img" , spk.SpeakerImgPath.FileName
                   });

                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    spk.SpeakerImgPath.CopyTo(fs);

                }

                //Save The Picture In folder

                var path = Path.Combine(
                 new string[]
                 {
                     _he.WebRootPath, "img", spk.SpeakerImgPath.FileName
                 });
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await spk.SpeakerImgPath.CopyToAsync(fs);
                    fs.Close();
                }

            }

           

            



            using (var db = new EventShowPlannerContext())
            {
                var speakerinfo = (from s in db.Speakers
                                   where s.SpeakerId == spk.SpeakerId
                                   select s).FirstOrDefault();

                //speakerinfo.SpeakerId = spk.SpeakerId;
                speakerinfo.SpeakerName = spk.SpeakerName;
                speakerinfo.Sage = spk.Sage;
                speakerinfo.Professional = spk.Professional;
                speakerinfo.Sjob = spk.Sjob;
                speakerinfo.Description = spk.Description;
                speakerinfo.ScreatedDate = spk.ScreatedDate;
                speakerinfo.Saddress = spk.Saddress;
                speakerinfo.CityId = spk.CityId;

                //speakerinfo.SpeakerImgPath = RelativePath;

                if (spk.SpeakerImgPath != null)
                {
                    speakerinfo.SpeakerImgPath = RelativePath;
                }

                db.Entry(speakerinfo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var ct = (from ab in db.Cities
                          where ab.CityId == speakerinfo.CityId
                          select ab).FirstOrDefault();
                ct.CityId = Convert.ToInt32(speakerinfo.CityId);

                db.Entry(ct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

            }

            return RedirectToRoute(new { controller = "Speaker", action = "Index" });
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            Speakerinfoindex spx = new Speakerinfoindex();
            using (var db = new EventShowPlannerContext())
            {
                var result = (from a in db.Speakers
                              join b in db.Cities
                              on a.CityId equals b.CityId
                              where a.SpeakerId == id
                              select new
                              {
                                  a.SpeakerId,
                                  a.SpeakerName,
                                  a.Sage,
                                  a.Saddress,
                                  a.Professional,
                                  a.Sjob,
                                  a.Description,
                                  a.CityId,
                                  b.CityName,
                                  a.SpeakerImgPath,
                              }).FirstOrDefault();

                spx.SpeakerId = result.SpeakerId;
               
                spx.SpeakerName = result.SpeakerName;
                spx.Sage = result.Sage;
                spx.Sjob = result.Sjob;
                spx.Professional = result.Professional;
                spx.Description = result.Description;
                spx.Saddress = result.Saddress;
                spx.CityId = result.CityId;
                spx.CityName = result.CityName;

                spx.SpeakerImgPath = result.SpeakerImgPath;

                return View(spx);
                              
            }      
        }

        [HttpPost]
        public IActionResult Delete (Speakerinfo abc)
        {
            using (var db = new EventShowPlannerContext())
            {
                var sp = (from s in db.Speakers
                          where s.SpeakerId == abc.SpeakerId
                          select s).FirstOrDefault();
                db.Entry(sp).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Speaker", action = "Index" });
            }
                
        }
    }  


}


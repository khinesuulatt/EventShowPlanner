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
using System.IO;
using System.Data.SqlTypes;
using System.Web;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Server;
using Microsoft.Extensions.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class EventController : Controller
    {

        private readonly EventShowPlannerContext _db;
        private readonly IWebHostEnvironment _he;

        public object Server { get; private set; }

        public EventController(EventShowPlannerContext db, IWebHostEnvironment he)
        {
            _he = he;
            _db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Eventinfoindex> events = new List<Eventinfoindex>();
            
            using (var db = new EventShowPlannerContext())
            {
                var result = (from e in db.Events
               
                              select new
                              {
                                  e.EventId,
                                  e.EventName,
                                  e.EventLocation,
                                  e.EventDate,
                                  e.EventStartTime,
                                  e.EventEndTime,
                                  e.MaxNumber,
                                  e.EventCategoryId,
                                  e.EventType,
                                  e.EventCreatedDate,

                                  e.EventImgPath,

                                  e.CityId,
                                
                              }).ToList();
                foreach(var e in result)
                {
                    events.Add(new Eventinfoindex
                    {
                        EventId = e.EventId,
                        EventName = e.EventName,
                        EventLocation = e.EventLocation,
                        EventDate = e.EventDate,
                        Date = Convert.ToDateTime(e.EventDate).ToString("MM/dd/yyyy"),
                        EventStartTime = e.EventStartTime,
                        EventEndTime = e.EventEndTime,
                        MaxNumber = e.MaxNumber,
                        EventCategoryId = e.EventCategoryId,
                        EventType = e.EventType,
                        EventCreatedDate = e.EventCreatedDate,

                        EventImgPath = e.EventImgPath,

                        CityId = e.CityId,
                    }) ;

                }

                

                return View(events);
            }
                
        }

        [HttpGet]
        public IActionResult Add()
        {
            

            List<EventCategory> eventCategories = new List<EventCategory>();
            eventCategories = (from catego in _db.EventCategories select catego).ToList();
            eventCategories.Insert(0, new EventCategory { EventCategoryId = 0, EcategoryName = "Select Category Name" });

            ViewBag.category = eventCategories;

            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            List<Speaker> speakers = new List<Speaker>();
            speakers = (from spk in _db.Speakers select spk).ToList();
            speakers.Insert(0, new Speaker { SpeakerId = 0, SpeakerName = "Select Speaker Name" });

            ViewBag.speaker = speakers;

            return View();
        }

        [HttpPost]

        public IActionResult Add(Eventinfo newevent)
        {
            
            string FilePath = "";
            string RelativePath = "";
            if (newevent.EventImgPath != null)
            {
                
                FilePath = Path.Combine(
                    new string[]
                    {
                _he.WebRootPath, "img" , newevent.EventImgPath.FileName
                    });

                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    newevent.EventImgPath.CopyTo(fs);

                }

                RelativePath = Path.Combine(
                    new string[]
                    {
                 "img" , newevent.EventImgPath.FileName
                    });
            }

            Event evt = new Event()
            {
                EventId = newevent.EventId,
                EventName = newevent.EventName,
                EventLocation = newevent.EventLocation,
                EventDate = newevent.EventDate,
                //EventDate = Convert.ToDateTime(newevent.EventDate).ToString("MM/dd/yyyy"),
                CityId = newevent.CityId,

                //EventStartTime = Convert.ToDateTime(newevent.EventStartTime).ToString("hh:mm tt"),

                EventStartTime = newevent.EventStartTime,
                EventEndTime = newevent.EventEndTime,


                MaxNumber = newevent.MaxNumber,
                EventCategoryId = newevent.EventCategoryId,
                EventType = newevent.EventType,
                EventCreatedDate = DateTime.Now,

                EventImgPath = RelativePath,
                
            };

            ViewBag.Message = "Selected Time: " + newevent;
            

            _db.Events.Add(evt);
            _db.Entry(evt).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _db.SaveChanges();

                   

            if (newevent.insertspeaker != null)
            {
                foreach (var a in newevent.insertspeaker)
                {
                    if (a.speakername != null)
                    {
                        EventDetail evd = new EventDetail()
                        {
                            SpeakerId = Convert.ToInt32(a.speakerid),
                            EventId = evt.EventId,
                            Edtitle = a.speakertitle,
                            Edcontent = a.speakercontent

                        };

                        _db.EventDetails.Add(evd);
                        _db.Entry(evd).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        _db.SaveChanges();
                    }
                   

                }
            }
            

            
            ViewBag.EventStartTime = "Start Time: ";
            ViewBag.EventEndTime = "End Time: ";

            return RedirectToRoute(new { controller = "Event" , action = "Index"});
        }


        [HttpGet]

        public IActionResult Edit(int id)
        {
            List<EventCategory> eventCategories = new List<EventCategory>();
            eventCategories = (from catego in _db.EventCategories select catego).ToList();
            eventCategories.Insert(0, new EventCategory { EventCategoryId = 0, EcategoryName = "Select Category Name" });

            ViewBag.category = eventCategories;

            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            List<Speaker> speakers = new List<Speaker>();
            speakers = (from spk in _db.Speakers select spk).ToList();
            speakers.Insert(0, new Speaker { SpeakerId = 0, SpeakerName = "Select Speaker Name" });

            ViewBag.speaker = speakers;


            Eventinfoindex efo = new Eventinfoindex();

            List<oldspeakerlist> speakerinfo = new List<oldspeakerlist>();
            using (var db = new EventShowPlannerContext())
            {
                var result = (from a in db.Events
                              join b in db.EventDetails
                              on a.EventId equals b.EventId
                              join c in db.Speakers
                              on b.SpeakerId equals c.SpeakerId
                              join e in db.EventCategories
                              on a.EventCategoryId equals e.EventCategoryId
                              where a.EventId == id

                              select new
                              {
                                  a.EventId,
                                  a.EventName,
                                  a.EventLocation,
                                  a.EventDate,
                                  a.EventStartTime,
                                  a.EventEndTime,
                                  a.MaxNumber,
                                  a.CityId,
                                  e.EventCategoryId,
                                  a.EventType,
                                  a.EventCreatedDate,
                                  a.EventImgPath,
                                  b.EventDetailId,
                                  c.SpeakerId,
                                  b.Edtitle,
                                  b.Edcontent,

                              }).FirstOrDefault();

                efo.EventId = result.EventId;
                efo.EventName = result.EventName;
                efo.EventLocation = result.EventLocation;
                efo.EventDate = result.EventDate;
                efo.EventStartTime = result.EventStartTime;
                efo.EventEndTime = result.EventEndTime;
                efo.MaxNumber = result.MaxNumber;
                efo.EventCategoryId = result.EventCategoryId;
                efo.EventType = result.EventType;
                efo.EventCreatedDate = result.EventCreatedDate;
                efo.EventImgPath = result.EventImgPath;
                efo.EventDetailId = result.EventDetailId;
                //efo.SpeakerId = result.SpeakerId;
                //efo.Edtitle = result.Edtitle;
                //efo.Edcontent = result.Edcontent;
                efo.CityId = result.CityId;

                var speaker_info = (from e in db.EventDetails
                                    join s in db.Speakers
                                    on e.SpeakerId equals s.SpeakerId
                                    where e.EventId == id
                                    select new
                                    {
                                        e.EventDetailId,
                                        e.SpeakerId,
                                        s.SpeakerName,
                                        e.Edtitle,
                                        e.Edcontent
                                    }).ToList();
                foreach (var item in speaker_info)
                {
                    speakerinfo.Add(new oldspeakerlist
                    {
                        oldspeakerid = Convert.ToString(item.SpeakerId),
                        oldspeakername = item.SpeakerName,
                        oldspeakertitle = item.Edtitle,
                        oldspeakercontent = item.Edcontent
                    });
                }

                efo.oldspeakerlists = speakerinfo;

                return View(efo);
            }
               
        }

        [HttpPost]
        public async Task<IActionResult> Edit (Eventinfo eventinfo , IFormFile photo)
            
        {
            string FilePath = "";
            string RelativePath = "";
            if (eventinfo.EventImgPath != null)
            {
                FilePath = Path.Combine(
                    new string[]
                    {
                _he.WebRootPath, "img" , eventinfo.EventImgPath.FileName
                    });

                RelativePath = Path.Combine(
                   new string[]
                   {
                 "img" , eventinfo.EventImgPath.FileName
                   });

                using (FileStream fs = System.IO.File.Create(FilePath))
                {
                    eventinfo.EventImgPath.CopyTo(fs);

                }

                //Save The Picture In folder
                var path = Path.Combine(
                     new string[]
                     {
                     _he.WebRootPath, "img", eventinfo.EventImgPath.FileName
                     });
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    await eventinfo.EventImgPath.CopyToAsync(fs);
                    fs.Close();
                }

            }


            



            using (var db = new EventShowPlannerContext())
            {
                var eto = (from a in db.Events                          
                           where a.EventId == eventinfo.EventId
                           select a).FirstOrDefault();

                eto.EventId = eventinfo.EventId;
                eto.EventName = eventinfo.EventName;
                eto.EventLocation = eventinfo.EventLocation;
                eto.EventDate = eventinfo.EventDate;
                //eto.EventCreatedDate = eventinfo.EventCreatedDate;
                eto.EventStartTime = eventinfo.EventStartTime;
                eto.CityId = eventinfo.CityId;
                eto.EventEndTime = eventinfo.EventEndTime;
                eto.MaxNumber = eventinfo.MaxNumber;
                eto.EventCategoryId = eventinfo.EventCategoryId;
                eto.EventType = eventinfo.EventType;

                if (eventinfo.EventImgPath != null)
                {
                    eto.EventImgPath = RelativePath;
                }
                    

                

                db.Entry(eto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var etc = (from c in db.EventCategories
                           where c.EventCategoryId == eventinfo.EventCategoryId
                           select c).FirstOrDefault();

                etc.EventCategoryId = Convert.ToInt32 (eventinfo.EventCategoryId);

                db.Entry(etc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var ct = (from ab in db.Cities
                          where ab.CityId == eventinfo.CityId
                          select ab).FirstOrDefault();
                ct.CityId = Convert.ToInt32(eventinfo.CityId);

                db.Entry(ct).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var etd = (from a in db.EventDetails
                           where a.EventDetailId == eventinfo.EventDetailId
                           select a).FirstOrDefault();

                etd.Edtitle = eventinfo.Edtitle;
                etd.Edcontent = eventinfo.Edcontent;

                db.Entry(etd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();


                var abcd = (from b in db.EventDetails
                            where b.EventId == eto.EventId
                            select b).ToList();
                foreach(var itemd in abcd)
                {
                    _db.Entry(itemd).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    _db.SaveChanges();
                }
                



                if (eventinfo.insertspeaker != null)
                {
                    foreach (var a in eventinfo.insertspeaker)
                    {
                        if (a.speakername != null)
                        {
                            EventDetail evd = new EventDetail()
                            {
                                SpeakerId = Convert.ToInt32(a.speakerid),
                                EventId = eto.EventId,
                                Edtitle = a.speakertitle,
                                Edcontent = a.speakercontent

                            };

                            _db.EventDetails.Add(evd);
                            _db.Entry(evd).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            _db.SaveChanges();
                        }

                    }
                }
                    if (eventinfo.oldspeakerlists != null)
                    {
                        foreach (var a in eventinfo.oldspeakerlists)
                        {
                            if (a.oldspeakername != null)
                            {
                                EventDetail evd = new EventDetail()
                                {
                                    SpeakerId = Convert.ToInt32(a.oldspeakerid),
                                    EventId = eto.EventId,
                                    Edtitle = a.oldspeakertitle,
                                    Edcontent = a.oldspeakercontent



                                };

                                _db.EventDetails.Add(evd);
                                _db.Entry(evd).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                                _db.SaveChanges();
                            }


                        }
                    }


                ViewBag.EventStartTime = "Start Time: ";
                ViewBag.EventEndTime = "End Time: ";

            }

            return RedirectToRoute(new { controller = "Event", action = "Index" });
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {

            List<EventCategory> eventCategories = new List<EventCategory>();
            eventCategories = (from catego in _db.EventCategories select catego).ToList();
            eventCategories.Insert(0, new EventCategory { EventCategoryId = 0, EcategoryName = "Select Category Name" });
            ViewBag.category = eventCategories;

            List<City> cities = new List<City>();
            cities = (from c in _db.Cities select c).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });
            ViewBag.city = cities;

            //List<Speaker> speakers = new List<Speaker>();
            //speakers = (from spk in _db.Speakers select spk).ToList();
            //speakers.Insert(0, new Speaker { SpeakerId = 0, SpeakerName = "Select Speaker Name" });

            //ViewBag.speaker = speakers;

            Eventinfoindex efo = new Eventinfoindex();

            List<speakerlist1> speakerinfo = new List<speakerlist1>();
            
            using (var db = new EventShowPlannerContext())
            {
                
                var result = (from a in db.Events
                              join e in db.EventCategories
                              on a.EventCategoryId equals e.EventCategoryId
                              join b in db.Cities
                              on a.CityId equals b.CityId

                              where a.EventId == id

                              select new
                              {
                                  a.EventId,
                                  a.EventName,
                                  a.EventLocation,
                                  a.EventDate,
                                  a.EventStartTime,
                                  a.EventEndTime,
                                  a.MaxNumber,                               
                                  a.EventType,
                                  a.EventCreatedDate,
                                  a.EventImgPath,
                                  e.EventCategoryId,
                                  a.CityId,
                                  b.CityName
                              }).FirstOrDefault();

                efo.EventId = result.EventId;
                efo.EventName = result.EventName;
                efo.EventLocation = result.EventLocation;
                efo.EventDate = result.EventDate;
                efo.EventCreatedDate = result.EventCreatedDate;
                efo.EventStartTime = result.EventStartTime;
                //DateTime tt = Convert.ToDateTime(efo.EventStartTime);
                //string start_time = tt.ToString("HH:mm tt");
                efo.EventEndTime = result.EventEndTime;
                efo.MaxNumber = result.MaxNumber; 
                efo.EventType = result.EventType;
                efo.EventCategoryId = result.EventCategoryId;
                efo.CityId = result.CityId;
                
                efo.EventImgPath = result.EventImgPath;

                var speaker_info = (from e in db.EventDetails
                                    join s in db.Speakers
                                    on e.SpeakerId equals s.SpeakerId
                                    where e.EventId == id
                                    select new
                                    {
                                        e.EventDetailId,
                                        e.SpeakerId,
                                        s.SpeakerName,
                                        e.Edtitle,
                                        e.Edcontent
                                    }).ToList();
                foreach(var item in speaker_info)
                {
                    speakerinfo.Add(new speakerlist1
                    {
                        speakername1 = item.SpeakerName,
                        speakertitle1 = item.Edtitle,
                        speakercontent1 = item.Edcontent
                    });
                }

                efo.insertspeaker = speakerinfo;


                return View(efo);
            }

        }

        //[HttpPost]
        //public IActionResult Detail(Eventinfo eventinfo)
        //{

        //    string FilePath = Path.Combine(
        //    new string[]
        //    {
        //        _he.WebRootPath, "img" , eventinfo.EventImgPath.FileName
        //    });
        //    using (FileStream fs = System.IO.File.Create(FilePath))
        //    {
        //        eventinfo.EventImgPath.CopyTo(fs);

        //    }
        //    using (var db = new EventShowPlannerContext())
        //    {
        //        var eto = (from a in db.Events
        //                   where a.EventId == eventinfo.EventId
        //                   select a).FirstOrDefault();

        //        eto.EventId = eventinfo.EventId;
        //        eto.EventName = eventinfo.EventName;
        //        eto.EventLocation = eventinfo.EventLocation;
        //        eto.EventDate = eventinfo.EventDate;
        //        eto.EventStartTime = eventinfo.EventStartTime;
        //        eto.EventEndTime = eventinfo.EventEndTime;
        //        eto.MaxNumber = eventinfo.MaxNumber;
        //        eto.EventCategoryId = eventinfo.EventCategoryId;
        //        eto.EventType = eventinfo.EventType;

        //        eto.EventImgPath = FilePath;

        //        db.Entry(eto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //        db.SaveChanges();

        //        var etc = (from c in db.EventCategories
        //                   where c.EventCategoryId == eventinfo.EventCategoryId
        //                   select c).FirstOrDefault();

        //        etc.EventCategoryId = Convert.ToInt32(eventinfo.EventCategoryId);

        //        db.Entry(etc).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //        db.SaveChanges();

        //        var etd = (from a in db.EventDetails
        //                   where a.EventDetailId == eventinfo.EventDetailId
        //                   select a).FirstOrDefault();
        //        etd.Edtitle = eventinfo.Edtitle;
        //        etd.Edcontent = eventinfo.Edcontent;

        //        db.Entry(etd).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //        db.SaveChanges();

        //    }

        //    return RedirectToRoute(new { controller = "Event", action = "Index" });
        //}

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Eventinfo abc = new Eventinfo();
            using (var db = new EventShowPlannerContext())
            {
                var result = (from a in db.Events
                              join b in db.EventDetails
                              on a.EventId equals b.EventId
                              join c in db.Speakers
                              on b.SpeakerId equals c.SpeakerId

                              where a.EventId == id
                              select new
                              {
                                  a.EventId,
                                  a.EventName,
                                  a.EventLocation,
                                  a.EventType,
                                  b.Edtitle,
                                  b.EventDetailId,
                                  b.SpeakerId,
                              }).FirstOrDefault();
                                abc.EventId = result.EventId;
                abc.EventName = result.EventName;
                abc.EventLocation = result.EventLocation;
                abc.EventType = result.EventType;
                abc.Edtitle = result.Edtitle;
                abc.EventDetailId = result.EventDetailId;
                abc.SpeakerId = result.SpeakerId;

                return View(abc);
                              
            }
        }

        [HttpPost]
        public IActionResult Delete (Eventinfo xyz)
        {
            using (var db = new EventShowPlannerContext())
            {
                var ez = (from a in db.Events
                          where a.EventId == xyz.EventId
                          select a).FirstOrDefault();

                db.Entry(ez).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();

                var yz = (from e in db.EventDetails
                          where e.EventDetailId == xyz.EventDetailId
                          select e).FirstOrDefault();

                db.Entry(yz).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();

                return RedirectToRoute(new { controller = "Event", action = "Index" });

            }
        }

    }

    public class HttpPostedFileBase
    {
    }
}


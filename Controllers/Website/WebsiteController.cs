using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventShow.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Data.SqlClient.Server;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Hosting;
using IronBarCode;

namespace EventShow.Controllers.Website
{
    public class WebsiteController : Controller
    {
        private readonly EventShowPlannerContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WebsiteController(EventShowPlannerContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;

            _webHostEnvironment = webHostEnvironment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            
            List<EventA> evt = new List<EventA>();
             
            using (var db = new EventShowPlannerContext())
             
            {
                var result = (from a in db.Events
                              join c in db.EventCategories
                              on a.EventCategoryId equals c.EventCategoryId
                              select new
                              {
                                  a.EventName,
                                  a.EventLocation,
                                  a.EventDate,
                                  a.EventStartTime,
                                  a.EventEndTime,
                                  a.EventImgPath,
                                  a.EventId,
                                  c.EcategoryName,
                                  c.EventCategoryId,
                                
                                  
                              }).ToList();

                //var websiteresult = 

                foreach (var abc in result)
                {
                    evt.Add(new EventA

                    {
                       EventName = abc.EventName,
                       EventLocation = abc.EventLocation,
                       EventId = abc.EventId,
                       EventImgPath = abc.EventImgPath,
                       event_date = Convert.ToDateTime(abc.EventDate).ToString("MM/dd/yyyy"),
                       start_time = Convert.ToDateTime(abc.EventStartTime).ToString("hh:mm tt"),
                       end_time = Convert.ToDateTime(abc.EventEndTime).ToString("hh:mm tt"),
                       EventCategoryId = abc.EventCategoryId,
                       EcategoryName = abc.EcategoryName,
                });
              
                }

                
                return View(evt);

            }
             
            
        }
   


        public IActionResult Event()
        {
            return View();
        }

        
        public IActionResult Event1()
        {
            

                return View();
            
        }

        public IActionResult Event2()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var result = _db.Users.ToList();

            var accresult = (from u in _db.Users
                             where u.UserName == user.UserName &&
                             u.Password == user.Password
                             select u).FirstOrDefault();

            if(accresult != null)
            {
               if(accresult.Active == false)
                {
                    return Json(new { success = false, useractive = false });
                }

                else
                {
                    string userid = Convert.ToString(accresult.UserId);
                    string username = accresult.UserName;

                    HttpContext.Session.SetString("UserId", userid);
                    HttpContext.Session.SetString("UserName", username);

                    var user_id = HttpContext.Session.GetString("UserId");
                    var user_name = HttpContext.Session.GetString("UserName");

                    return Json(new { success = true, UserId = accresult.UserId, UserName = accresult.UserName });
                }           

            }

            else
            {
                return Json(new { success = false });
            }
            
        }

        public IActionResult Speaker(int id)
        {
            Speaker spkr = new Speaker();
            using (var db = new EventShowPlannerContext())
            {
                var speakerbio = (from s in db.Speakers

                                  where s.SpeakerId == id
                                  select new
                                  {
                                      s.SpeakerId,
                                      s.SpeakerName,
                                      s.SpeakerImgPath,
                                      s.Saddress,
                                      s.Professional,
                                      s.Sage,
                                      s.Sjob,
                                      s.Description,
                                      
                                     
                                  }).FirstOrDefault();

                spkr.SpeakerId = speakerbio.SpeakerId;
                spkr.SpeakerName = speakerbio.SpeakerName;
                spkr.SpeakerImgPath = speakerbio.SpeakerImgPath;
                spkr.Sjob = speakerbio.Sjob;
                spkr.Sage = speakerbio.Sage;
                spkr.Saddress = speakerbio.Saddress;
                spkr.Description = speakerbio.Description;
                spkr.Professional = speakerbio.Professional;
                
            }

                return View(spkr);
        }

        [HttpGet]
        public IActionResult Kumo(int id)
        {
            EventOne evt1 = new EventOne();
            List<speakerlistone> spk1 = new List<speakerlistone>();

            using (var db = new EventShowPlannerContext())

            {
                var result = (from a in db.Events
                              
                              join c in db.EventCategories
                              on a.EventCategoryId equals c.EventCategoryId
                              
                              join e in db.Cities
                              on a.CityId equals e.CityId
                              where a.EventId == id
                              select new
                              {
                                  a.EventName,
                                  e.CityName,
                                  a.EventLocation,
                                  a.EventDate,
                                  a.EventStartTime,
                                  a.EventEndTime,
                                  a.EventImgPath,
                                  a.EventId,
                                  c.EcategoryName,                     
                                 
                              }).FirstOrDefault();

                evt1.EventId = result.EventId;
                evt1.EventName = result.EventName;
                evt1.CityName = result.CityName;
                evt1.EventLocation = result.EventLocation;
                evt1.EventImgPath = result.EventImgPath;
                evt1.EcategoryName = result.EcategoryName;
                evt1.event_date = Convert.ToDateTime(result.EventDate).ToString("MM/dd/yyyy");
                evt1.start_time = Convert.ToDateTime(result.EventStartTime).ToString("hh:mm tt");
                evt1.end_time = Convert.ToDateTime(result.EventEndTime).ToString("hh:mm tt");

                var user_id = HttpContext.Session.GetString("UserId");
                var user_name = HttpContext.Session.GetString("UserName");

                evt1.loginuserid = Convert.ToInt32(user_id);
                evt1.loginusername = user_name;

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
                                        e.Edcontent,
                                        s.SpeakerImgPath,
                                        s.Saddress,
                                        s.Professional,
                                        s.Sage,
                                        s.Sjob,
                                        s.Description,
                                    }).ToList();

                foreach (var item in speaker_info)
                {
                    spk1.Add(new speakerlistone
                    {
                        speakerid = item.SpeakerId,
                        speakername = item.SpeakerName,
                        SpeakerImgPath = item.SpeakerImgPath,
                        speakertitle = item.Edtitle,
                        speakercontent = item.Edcontent,
                        speakerdescription = item.Description,
                        speakeraddress = item.Saddress,
                        speakerage = item.Sage,
                        speakerjob = item.Sjob,
                        speakerprofessional = item.Professional,
                    });
                }

                evt1.speakerlist = spk1;

                return View(evt1);
            }
        }

        [HttpPost]
        public IActionResult Transaction(EnrollUser enrollUser)
        {

            using (var db = new EventShowPlannerContext())
            {
                if (enrollUser.loginuserid != 0)
                {
                    Transaction t = new Transaction()
                    {

                        EventId = enrollUser.EventId,
                        UserId = enrollUser.loginuserid,
                        Qrcode = enrollUser.loginuserid.ToString() + enrollUser.EventId.ToString(),
                        //Qrimgpath = enrollUser.Qrimgpath,
                        TransactionDate = DateTime.Now,
                        StatusId = 1,

                    };

                    try
                    {
                        GeneratedBarcode qrcode1 = QRCodeWriter.CreateQrCode(enrollUser.EventId.ToString() + enrollUser.loginuserid.ToString(), 200);
                        qrcode1.AddBarcodeValueTextBelowBarcode();
                        qrcode1.SetMargins(10);
                        qrcode1.ChangeBarCodeColor(BarcodeColor: IronSoftware.Drawing.Color.Black);
                        string filePath = "GeneratedQRCode";
                        string Time = DateTime.Now.ToString("dd MM yyyy HH:mm:ss"); // as u like
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string filePath1 = Path.Combine(_webHostEnvironment.WebRootPath, filePath + "/QR-" + Time + ".png");
                        qrcode1.SaveAsPng(filePath1);

                        string filename = Path.GetFileName(filePath1);
                        //string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/" + filePath + "/" + filename;
                        string imageUrl = filePath + "/" + filename;
                        ViewBag.QrCodeUri = imageUrl;


                        t.Qrimgpath = imageUrl;
                        _db.Transactions.Add(t);
                        _db.SaveChanges();


                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    return Json(new { success = true});
                }

                else
                {
                    return Json(new { success = false });
                }

               

            }

            
        }

        public IActionResult Test()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            List<City> cities = new List<City>();
            cities = (from city in _db.Cities select city).ToList();
            cities.Insert(0, new City { CityId = 0, CityName = "Select City Name" });

            ViewBag.city = cities;

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUser registerUser)
        {
            User r = new User()
            {
                UserName = registerUser.UserName,
                UserAddress = registerUser.UserAddress,
                CityId = registerUser.CityId,
                UserAge = registerUser.UserAge,
                Gender = registerUser.Gender,
                JobTitle = registerUser.JobTitle,
                Password = registerUser.Password,
                
                EmailAddress = registerUser.EmailAddress,
                PhoneNumber = registerUser.PhoneNumber,
                UcreatedDate = DateTime.Now,
                Active = true,
            };

            _db.Users.Add(r);
            _db.SaveChanges();

            return RedirectToRoute(new { controller = "Website", action = "Login" });

        }

        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendText(Userfeedbacks userfeedbacks)
        {
            var user_id = 0;
            Feedback f = new Feedback()
            {
             
                FName = userfeedbacks.FName,
                FDate = DateTime.Now,
                UserId = Convert.ToInt32(user_id),
                
            };

            _db.Feedbacks.Add(f);
            _db.SaveChanges();
       
            return Json(new { success = true });
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EventShow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting.Internal;
using IronBarCode;
using ZXing;
using BitMiracle.LibTiff.Classic;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class TransactionController : Controller
    {
        private readonly EventShowPlannerContext _db;
        private IWebHostEnvironment _env;
        private ITempDataProvider _tempDataProvider;

        private IRazorViewEngine _razorViewEngine;

        public TransactionController(EventShowPlannerContext db, IWebHostEnvironment env, IRazorViewEngine engine, ITempDataProvider tempDataProvider)
        {
            _db = db;
            _env = env;
            _razorViewEngine = engine;
            _tempDataProvider = tempDataProvider;
        }


        public IActionResult Index()
        {
            List<EnrollUser> transactions = new List<EnrollUser>();
            using (var db = new EventShowPlannerContext())
            {
                var tran = (from t in db.Transactions
                            join e in db.Events
                            on t.EventId equals e.EventId
                            join u in db.Users
                            on t.UserId equals u.UserId
                            join d in db.EventCategories
                            on e.EventCategoryId equals d.EventCategoryId

                            select new
                            {
                                t.TransactionId,
                                t.EventId,
                                e.EventName,
                                t.UserId,
                                u.UserName,
                                t.Qrcode,
                                t.Qrimgpath,
                                t.TransactionDate,
                                t.StatusId,
                            }).ToList();
                foreach (var s in tran)
                {
                    transactions.Add(new Models.EnrollUser
                    {
                        TransactionId = s.TransactionId,
                        EventId = s.EventId,
                        EventName = s.EventName,
                        UserId = s.UserId,
                        UserName = s.UserName,
                        Qrcode = s.Qrcode,
                        Qrimgpath = s.Qrimgpath,
                        TransactionDate = DateTime.Now,
                        StatusId = s.StatusId,

                    });

                }
            }
            return View(transactions);
        }
       
        public async Task<IActionResult> ConfirmAsync(int id)
        {
            Transaction t = new Transaction();
            using (var db = new EventShowPlannerContext())
            {
                var result1 = (from a in db.Transactions
                               join b in db.Users
                               on a.UserId equals b.UserId
                               join c in db.Events
                               on a.EventId equals c.EventId
                               join d in db.EventCategories
                               on c.EventCategoryId equals d.EventCategoryId
                               where a.TransactionId == id

                               select new EnrollUser
                               {
                                  TransactionId = a.TransactionId,
                                  EventId =  a.EventId,
                                  UserId =  a.UserId,
                                  StatusId = a.StatusId,
                                  Qrimgpath = a.Qrimgpath,
                                  Qrcode = a.Qrcode,
                                  EmailAddress = b.EmailAddress,
                                  EventName = c.EventName,
                                  EventLocation =  c.EventLocation,
                                  EventDate = c.EventDate,
                                  EventStartTime = c.EventStartTime,
                                  EventEndTime = c.EventEndTime,
                                  EcategoryName = d.EcategoryName,
                                  UserName = b.UserName,
                                  EDate = Convert.ToDateTime(c.EventDate).ToString("MM/dd/yyyy"),
                                  ESTime = Convert.ToDateTime(c.EventStartTime).ToString("hh:mm tt"),
                                  EETime = Convert.ToDateTime(c.EventEndTime).ToString("hh:mm:tt")
                               }).FirstOrDefault();

                


                if (result1 != null)
                {
                    string fromMail = "eventplannerproject122022@gmail.com";
                    string fromPassword = "ehmkidnmkabvlgag";

                    MailMessage message = new MailMessage();

                    

                    var TempPath = _env.WebRootPath; //get wwwroot folder

                    //var emailBody = TempPath + Path.DirectorySeparatorChar.ToString() + "Templates" + Path.DirectorySeparatorChar.ToString() + "EmailTemplate" +
                    //    Path.DirectorySeparatorChar.ToString() + "EventEmail.html";


                    //var emailBody = TempPath + Path.DirectorySeparatorChar.ToString() + "Transaction" + Path.DirectorySeparatorChar.ToString() + "EventEmail.cshtml";

                    //StreamReader str = new StreamReader(emailBody);
                    //string Mailtext = str.ReadToEnd();
                    //str.Close();

                    //var builder = new BodyBuilder();
                    //using (StreamReader streamReader = System.IO.File.OpenText(pathToFile))
                    //{
                    //    builder.HtmlBody = Source
                    //}

                    var serverpath = Path.Combine(new[] { _env.WebRootPath, result1.Qrimgpath });
                    var contents = System.IO.File.OpenRead(serverpath);
                    //result1.Qrimgpath = $"data:image/png;base64,{Convert.ToBase64String(contents)}";

                    using (StringWriter sw = new StringWriter())
                    {
                        var viewResult = _razorViewEngine.FindView(ControllerContext, "EventEmail", false);

                        if (viewResult.View == null)
                        {
                            //return string.Empty;
                        }

                        var metadataProvider = new EmptyModelMetadataProvider();
                        var msDictionary = new ModelStateDictionary();
                        var viewDataDictionary = new ViewDataDictionary(metadataProvider, msDictionary);

                        viewDataDictionary.Model = result1;

                        var tempDictionary = new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider);
                        var viewContext = new ViewContext(
                            ControllerContext,
                            viewResult.View,
                            viewDataDictionary,
                            tempDictionary,
                            sw,
                            new HtmlHelperOptions()
                        );

                        await viewResult.View.RenderAsync(viewContext);


                       
                        message.From = new MailAddress(fromMail);
                        message.To.Add(new MailAddress(result1.EmailAddress));
                        message.Body =  sw.ToString();
                        message.IsBodyHtml = true;

                        message.Attachments.Add(new Attachment(contents,"YourQRCode.png"));
                        

                        message.Subject = "Send mail Asp,net core web api";

                        var smtpClient = new SmtpClient("smtp.gmail.com")
                        {
                            Port = 587,
                            Credentials = new NetworkCredential(fromMail, fromPassword),
                            EnableSsl = true,

                        };

                        smtpClient.Send(message);
                        smtpClient.Dispose();
                        contents.Close();

                    }

                }

                var ab = (from a in db.Transactions
                          where a.TransactionId == id
                          select a).FirstOrDefault();

                ab.StatusId = 2;
                db.Entry(ab).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                var user_id = db.Transactions.Where(a => a.TransactionId == id).Select(w => w.UserId).FirstOrDefault();

                var count = (from c in db.Users
                             where c.UserId == user_id
                             select c).FirstOrDefault();

                int e_Count = Convert.ToInt32(count.EventCount);
                count.EventCount = e_Count + 1;

                db.Entry(count).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();


                return RedirectToRoute(new { controller = "Transaction", action = "Index" });
            }

            //public IActionResult Status(int id)
            //{
            //    using (var db = new EventShowPlannerContext())
            //    {
            //        var sta = (from s in db.Transactions
            //                   where s.TransactionId == id
            //                   select s).FirstOrDefault();
            //        sta.Progress = 
            //    }
            //}
        }

        
        public IActionResult Confirmed(int id)
        {
            using (var db = new EventShowPlannerContext())
            {

                var tr = (from z in db.Transactions
                          where z.TransactionId == id
                          select z).FirstOrDefault();

                tr.StatusId = 1;

                db.Entry(tr).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute(new { controller = "Transaction", action = "Index" });
            }

        }

        public IActionResult Test()
        {
            List<EnrollUser> transactions = new List<EnrollUser>();
            using (var db = new EventShowPlannerContext())
            {
                var tran = (from t in db.Transactions
                            join e in db.Events
                            on t.EventId equals e.EventId
                            join u in db.Users
                            on t.UserId equals u.UserId

                            select new
                            {
                                t.TransactionId,
                                t.EventId,
                                e.EventName,
                                t.UserId,
                                u.UserName,
                                t.Qrcode,
                                t.Qrimgpath,
                                t.TransactionDate,
                                t.StatusId,
                            }).ToList();

                foreach (var s in tran)
                {
                    transactions.Add(new Models.EnrollUser
                    {
                        TransactionId = s.TransactionId,
                        EventId = s.EventId,
                        EventName = s.EventName,
                        UserId = s.UserId,
                        UserName = s.UserName,
                        Qrcode = s.Qrcode,
                        Qrimgpath = s.Qrimgpath,
                        TransactionDate = DateTime.Now,
                        StatusId = s.StatusId,

                    });

                }
            }
            return View(transactions);
            
        }

    }
}


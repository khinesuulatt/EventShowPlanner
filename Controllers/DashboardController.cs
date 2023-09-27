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
using System.Dynamic;
using ZXing;
using Microsoft.EntityFrameworkCore;

namespace EventShow.Controllers
{
    public class DashboardController : Controller
    {
        private readonly EventShowPlannerContext _db;
        

        public DashboardController(EventShowPlannerContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            
            
                Dashboardcounts dbc = new Dashboardcounts();
                List<Eventtable> event_table = new List<Eventtable>();
                List<Trantable> trans_table = new List<Trantable>();
                using (var db = new EventShowPlannerContext())

                {
                    var ucount = (from u in db.Users
                                  where u.Active == true
                                  select new
                                  {
                                      u.UserId,
                                      u.UserName,
                                  }).ToList();

                    dbc.usercount = ucount.Count();


                    var scount = (from s in db.Speakers
                                  where s.Active == true
                                  select new
                                  {
                                      s.SpeakerId,
                                      s.SpeakerName,
                                  }).ToList();

                    dbc.speakercount = scount.Count();


                    DateTime today_date = DateTime.Now;
                    DateTime firstDayOfMonth = new DateTime(today_date.Year, today_date.Month, 1);
                    DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                    var pecount = (from p in db.Events
                                   where p.EventDate < firstDayOfMonth
                                   select new
                                   {
                                       p.EventId,
                                       p.EventName,
                                   }).ToList();

                    dbc.preeventcount = pecount.Count();


                    var ecount = (from e in db.Events
                                  where e.EventDate >= firstDayOfMonth && e.EventDate <= lastDayOfMonth
                                  select new
                                  {
                                      e.EventId,
                                      e.EventName,
                                      e.EventDate,
                                  }).ToList();

                    dbc.eventcount = ecount.Count();

                    var e_count = (from ec in db.Events
                                   where ec.EventDate >= firstDayOfMonth && ec.EventDate <= lastDayOfMonth
                                   select new
                                   {
                                       ec.EventId,
                                       ec.EventName,
                                       ec.EventDate,
                                   }).ToList();
                    foreach (var t in e_count)
                    {
                        event_table.Add(new Eventtable
                        {

                            EventId = t.EventId,
                            EventName = t.EventName,
                            //Event_Date = t.EventDate,
                            Event_Date = Convert.ToDateTime(t.EventDate).ToString("MM/dd/yyyy"),
                        });
                    }

                    dbc.eventtables = event_table;

                    var t_count = (from ec in db.Transactions
                                   join u in db.Users
                                   on ec.UserId equals u.UserId
                                   where ec.TransactionDate.Value.Date == DateTime.Today.Date

                                   select new
                                   {
                                       ec.TransactionId,
                                       u.UserName,
                                       ec.TransactionDate,
                                   }).ToList();

                    foreach (var c in t_count)
                    {
                        trans_table.Add(new Trantable
                        {

                            TransactionId = c.TransactionId,
                            User_Name = c.UserName,
                            Transaction_Date = c.TransactionDate,

                        });
                    }

                    dbc.trantables = trans_table;

                    return View(dbc);
                }
            

            
            
        }
    }
}


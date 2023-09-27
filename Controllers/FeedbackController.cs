using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventShow.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventShow.Controllers
{
    public class FeedbackController : Controller
    {
        // GET: /<controller>/
    

      
        public IActionResult Index()        {
            List<Userfeedbacks> feedbacks = new List<Userfeedbacks>();
         
            using (var db = new EventShowPlannerContext())
            {
                var fb = (from f in db.Feedbacks
                          select new
                          {
                              f.FeedbackId,
                              f.FName,
                              f.FDate
                             
                              
                          }).ToList();

                foreach (var e in fb)
                {
                    feedbacks.Add(new Userfeedbacks
                    {

                        FeedbackId = e.FeedbackId,
                        FName = e.FName,
                        FDate = e.FDate,

                        
                    });
                }


                return View(feedbacks);
            }
        }
    }
}


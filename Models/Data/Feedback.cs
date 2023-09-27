using System;
using System.ComponentModel.DataAnnotations;
namespace EventShow.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string? FName { get; set; }
        public DateTime? FDate { get; set; }
        public int? UserId { get; set; }
    }
}


using System;
namespace EventShow.Models
{
    public class Userfeedbacks
    {
        public int FeedbackId { get; set; }
        public string? FName { get; set; }
        public DateTime? FDate { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}


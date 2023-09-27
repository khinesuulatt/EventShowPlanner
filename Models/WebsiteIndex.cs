using System;
namespace EventShow.Models
{
    public class WebsiteIndex
    {
        public int UserId { get; set; }
        public List<EventA>? EventAs { get; set; }
        public string? UserName { get; set; }
    }
}


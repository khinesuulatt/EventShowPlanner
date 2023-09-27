using System;
using System.ComponentModel.DataAnnotations;
namespace EventShow.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string? AdminName { get; set; }
        public string? AdminPassword { get; set; }
        public bool? Active { get; set; }
    }
}



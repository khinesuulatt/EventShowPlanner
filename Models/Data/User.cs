using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAddress { get; set; }
        public int? CityId { get; set; }
        public int? UserAge { get; set; }
        public string? Gender { get; set; }
        public string? JobTitle { get; set; }
        public DateTime? UcreatedDate { get; set; }
        public bool? Active { get; set; }
        public int? EventCount { get; set; }
        public string? Password { get; set; }
        public string? EmailAddress { get; set; }
        public string? PhoneNumber { get; set; }
    
    }
}

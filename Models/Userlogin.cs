using System;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public class Userlogin
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }   
        public string? Password { get; set; }
       
    }
}


using System;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public class RegisterUser
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

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password did not match")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
    }
}


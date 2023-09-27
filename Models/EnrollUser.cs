using System;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public class EnrollUser
    {
        public int TransactionId { get; set; }

        public int? EventId { get; set; }
        public string? EventName { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public string? Qrcode { get; set; }
        public string? Qrimgpath { get; set; }
        public DateTime? TransactionDate { get; set; }

        public int? StatusId { get; set; }
        
        public string? StatusName { get; set; }

        public int? loginuserid { get; set; }
        public string? loginusername { get; set; }

        public string? EventLocation { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EventDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:tt}", ApplyFormatInEditMode = true)]
        public DateTime? EventStartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm\\:tt}", ApplyFormatInEditMode = true)]
        public DateTime? EventEndTime { get; set; }

        public string? EmailAddress { get; set; }

        public string? EcategoryName { get; set; }

        public string? EDate { get; set; }
        public string? ESTime { get; set; }
        public string? EETime { get; set; }

    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public partial class EventA
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }
        public int? MaxNumber { get; set; }
        public int? EventCategoryId { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventCreatedDate { get; set; }
        public string? EventImgPath { get; set; }
        public int? CityId { get; set; }

        public string? event_date { get; set; }


        public string? start_time { get; set; }
        public string? end_time { get; set; }
   
        public string? EcategoryName { get; set; }

        public int UserId { get; set; }
        public string? UserName { get; set; }

        
        public string? FName { get; set; }
        
    }
}

using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class Event
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? EventStartTime { get; set; }
        public DateTime? EventEndTime { get; set; }
        public int? MaxNumber { get; set; }
        public int? EventCategoryId { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventCreatedDate { get; set; }
        public string? EventImgPath { get; set; }
        public int? CityId { get; set; }
    }
}

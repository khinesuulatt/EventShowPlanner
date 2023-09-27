using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class EventDetail
    {
        public int EventDetailId { get; set; }
        public int? EventId { get; set; }
        public int? SpeakerId { get; set; }
        public string? Edtitle { get; set; }
        public string? Edcontent { get; set; }
    }
}

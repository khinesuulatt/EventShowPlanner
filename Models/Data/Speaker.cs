using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class Speaker
    {
        public int SpeakerId { get; set; }
        public string? SpeakerName { get; set; }
        public int? Sage { get; set; }
        public string? Professional { get; set; }
        public string? Sjob { get; set; }
        public string? Description { get; set; }
        public string? Saddress { get; set; }
        public int? CityId { get; set; }
        public DateTime? ScreatedDate { get; set; }
        public bool Active { get; set; }
        public string? SpeakerImgPath { get; set; }
    }
}

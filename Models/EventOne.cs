using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public partial class EventOne
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

        public string? SpeakerName { get; set; }
        public string? Edtitle { get; set; }
        public string? Edcontent { get; set; }

        public string? CityName { get; set; } = null!;

        public string? SpeakerImgPath { get; set; }
        
        public int? Sage { get; set; }
        public string? Professional { get; set; }
        public string? Sjob { get; set; }
        public string? Description { get; set; }
        public string? Saddress { get; set; }

        public int? loginuserid { get; set; }
        public string? loginusername { get; set; }

        public int TransactionId { get; set; }
     
        public int? UserId { get; set; }

        public string? Qrcode { get; set; }
        public string? Qrimgpath { get; set; }
        public DateTime? TransactionDate { get; set; }

        public int? StatusId { get; set; }

        public string? StatusName { get; set; }

        public List<speakerlistone> speakerlist { get; set; }
        
    }

    public partial class speakerlistone
    {
        [Key]
        public int? speakerid { get; set; }
        public string speakername { get; set; }
        public string speakertitle { get; set; }
        public string speakercontent { get; set; }
        public string SpeakerImgPath { get; set; }
        public string speakerprofessional { get; set; }
        public string speakerdescription { get; set; }
        public int? speakerage { get; set; }
        public string speakerjob { get; set; }
        public string speakeraddress { get; set; }
    }
    
}

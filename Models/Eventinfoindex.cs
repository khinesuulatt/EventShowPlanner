using System;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public class Eventinfoindex
    {
        [Key]
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventDate { get; set; }

        public string Date { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]

        public DateTime? EventStartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]

        public DateTime? EventEndTime { get; set; }


        public int? MaxNumber { get; set; }
        public int? EventCategoryId { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventCreatedDate { get; set; }

        public string? EventImgPath { get; set; }

        public int EventDetailId { get; set; }

        public int? CityId { get; set; }


        public List<speakerlist1> insertspeaker { get; set; }

        public List<oldspeakerlist> oldspeakerlists { get; set; }

        public int? SpeakerId { get; set; }
        public string? SpeakerName { get; set; }
        public string? Edtitle { get; set; }
        public string? Edcontent { get; set; }

    }
    public partial class speakerlist1
    {
        [Key]
        public string speakerid1 { get; set; }
        public string speakername1 { get; set; }
        public string speakertitle1 { get; set; }
        public string speakercontent1 { get; set; }

    }

    public partial class oldspeakerlist
    {
        [Key]
        public string oldspeakerid { get; set; }
        public string oldspeakername { get; set; }
        public string oldspeakertitle { get; set; }
        public string oldspeakercontent { get; set; }

    }
}


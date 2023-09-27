using System;
using System.ComponentModel.DataAnnotations;

namespace EventShow.Models
{
    public class Eventinfo
    {
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }


        public DateTime? EventDate { get; set; }

        public string Date { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]
        //public DateTime? EventStartTime { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm}")]
        //public DateTime? EventEndTime { get; set; }

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


        public int? MaxNumber { get; set; }
        public int? EventCategoryId { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventCreatedDate { get; set; }

        public IFormFile EventImgPath { get; set; }

        public int Filepath { get; internal set; }

        public int EventDetailId { get; set; }
        public int? CityId { get; set; }


        public int? SpeakerId { get; set; }

        public string? SpeakerName { get; set; }
        public string? Edtitle { get; set; }
        public string? Edcontent { get; set; }

        public List<speakerlist> insertspeaker { get; set; }

        public List<oldspeakerlist1> oldspeakerlists { get; set; }


        

    }

    public partial class speakerlist
    {
        [Key]
        public string speakerid { get; set; }
        public string speakername { get; set; }
        public string speakertitle { get; set; }
        public string speakercontent { get; set; }

    }

    public partial class oldspeakerlist1
    {
        [Key]
        public string oldspeakerid { get; set; }
        public string oldspeakername { get; set; }
        public string oldspeakertitle { get; set; }
        public string oldspeakercontent { get; set; }

    }


}


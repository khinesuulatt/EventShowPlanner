using System;
namespace EventShow.Models
{
    public class Dashboardcounts
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int? CityId { get; set; }  
        public bool? Active { get; set; }
        public int? EventCount { get; set; }
     

        public int SpeakerId { get; set; }
        public string? SpeakerName { get; set; }

        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }
        public DateTime? EventDate { get; set; }
        public DateTime? EventStartTime { get; set; }
        public DateTime? EventEndTime { get; set; }

        public int? usercount { get; set; }

        public int? speakercount { get; set; }

        public int? preeventcount { get; set; }

        public int? eventcount { get; set; }

        public List<Eventtable>? eventtables { get; set; }

        public List<Trantable>? trantables { get; set; }
    }

    public class Eventtable
    {
        public int? EventId { get; set; }
        public string? EventName { get; set; }
        public string? Event_Date { get; set; }
    }

    public class Trantable
    {
        public int? TransactionId { get; set; }
        public string? User_Name { get; set; }
        public DateTime? Transaction_Date { get; set; }
    }
}


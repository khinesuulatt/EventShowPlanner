﻿using System;
using System.ComponentModel.DataAnnotations;
namespace EventShow.Models
{
    public class Speakerinfo
    {
        public int? SpeakerId { get; set; }
        public string? SpeakerName { get; set; }
        public int? Sage { get; set; }
        public string? Professional { get; set; }
        public string? Sjob { get; set; }
        public string? Description { get; set; }
        public string? Saddress { get; set; }
        public int? CityId { get; set; }
        public string? CityName { get; set; }
        public DateTime? ScreatedDate { get; set; }

        public IFormFile SpeakerImgPath { get; set; }

        public bool Active { get; set; }

        public int Filepath { get; internal set; }
    }
}


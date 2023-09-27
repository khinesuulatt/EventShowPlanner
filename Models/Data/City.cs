using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class City
    {
        public int? CityId { get; set; }
        public string? CityName { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime CcreatedDate { get; set; }
    }
}

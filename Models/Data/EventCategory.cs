using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class EventCategory
    {
        public int EventCategoryId { get; set; }
        public string? EcategoryName { get; set; }
        public DateTime? EccreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace EventShow.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? EventId { get; set; }
        public int? UserId { get; set; }
        public string? Qrcode { get; set; }
        public string? Qrimgpath { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? StatusId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ExamineFillInRecord
    {
        public int SeqNo { get; set; }
        public Guid Id { get; set; }
        public Guid ExamineId { get; set; }
        public string Ip { get; set; } = null!;
        public DateTime CreateDate { get; set; }
    }
}

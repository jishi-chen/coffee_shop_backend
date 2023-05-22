using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ApplicationRecord
    {
        public int SeqNo { get; set; }
        public Guid RegId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ApplicationFieldId { get; set; }
        public string? FilledText { get; set; }
        public string? AnswerText { get; set; }
        public string? Remark { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class DocumentRecord
    {
        public long SeqNo { get; set; }
        public string RegId { get; set; } = null!;
        public string DocumentId { get; set; } = null!;
        public string DocumentFieldId { get; set; } = null!;
        public string? FilledText { get; set; }
        public string? MemoText { get; set; }
        public string? Remark { get; set; }
    }
}

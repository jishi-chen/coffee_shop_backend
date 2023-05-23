using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class DocumentRecord
    {
        public long SeqNo { get; set; }
        public long RegId { get; set; }
        public long DocumentId { get; set; }
        public long DocumentFieldId { get; set; }
        public string? FilledText { get; set; }
        public string? MemoText { get; set; }
        public string? Remark { get; set; }
    }
}

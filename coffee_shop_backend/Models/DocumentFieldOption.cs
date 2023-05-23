using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class DocumentFieldOption
    {
        public string Id { get; set; } = null!;
        public string DocumentFieldId { get; set; } = null!;
        public string OptionName { get; set; } = null!;
        public byte MemoType { get; set; }
        public int Sort { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class ApplicationFieldOption
    {
        public Guid Id { get; set; }
        public Guid ApplicationFieldId { get; set; }
        public string OptionName { get; set; } = null!;
        public byte MemoType { get; set; }
        public int Sort { get; set; }
    }
}

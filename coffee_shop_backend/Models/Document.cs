using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class Document
    {
        public string? Id { get; set; } = string.Empty;
        public string CsId { get; set; } = string.Empty;
        public string Caption { get; set; } = null!;
        public bool IsEnabled { get; set; } = true;
        public int Sort { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.MaxValue;
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
        public int Hits { get; set; }
        public string Creator { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? Updator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

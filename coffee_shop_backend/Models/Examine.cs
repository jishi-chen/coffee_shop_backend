﻿using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class Examine
    {
        public Guid Id { get; set; }
        public string ExamineNo { get; set; } = null!;
        public string Caption { get; set; } = null!;
        public bool IsEnabled { get; set; }
        public short Sort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? HeadText { get; set; }
        public string? FooterText { get; set; }
        public int Hits { get; set; }
        public string Creator { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string? Updator { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

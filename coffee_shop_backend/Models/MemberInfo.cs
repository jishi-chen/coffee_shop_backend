using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class MemberInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telephone { get; set; }
        public string Cellphone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public bool IsDelete { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class Reg
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

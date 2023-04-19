using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class AddressCity
    {
        public Guid Id { get; set; }
        public string CityName { get; set; } = null!;
        public int SortIndex { get; set; }
    }
}

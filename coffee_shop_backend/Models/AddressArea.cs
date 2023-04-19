using System;
using System.Collections.Generic;

namespace coffee_shop_backend.Models
{
    public partial class AddressArea
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string AreaName { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int SortIndex { get; set; }
    }
}

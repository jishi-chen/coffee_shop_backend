using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class AddressArea
{
    public int Id { get; set; }

    public int CityId { get; set; }

    public string AreaName { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public int SortIndex { get; set; }
}

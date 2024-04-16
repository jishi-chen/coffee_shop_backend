using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class AddressCity
{
    public Guid Id { get; set; }

    public string CityName { get; set; } = null!;

    public int SortIndex { get; set; }
}

using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class Category
{
    public int CategoryId { get; set; }

    public int? ModuleId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int Sort { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateDate { get; set; }

    public int Updator { get; set; }
}

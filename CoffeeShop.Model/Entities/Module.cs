using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class Module
{
    public int ModuleId { get; set; }

    public string ModuleName { get; set; } = null!;

    public int Sort { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateDate { get; set; }

    public int Updator { get; set; }
}

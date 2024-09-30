using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class Tenant
{
    public int TenantId { get; set; }

    public string TenantName { get; set; } = null!;

    public string? Address { get; set; }

    public string ContactEmail { get; set; } = null!;

    public string ContactName { get; set; } = null!;

    public string ContactPhone { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public int Creator { get; set; }

    public DateTime CreateDate { get; set; }

    public int? Updator { get; set; }

    public DateTime? UpdateDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string IdentityString { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}

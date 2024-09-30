using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class User
{
    public int UserId { get; set; }

    public int TenantId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? AddressId { get; set; }

    public string? Address { get; set; }

    public string PasswordHash { get; set; } = null!;

    public byte Role { get; set; }

    public byte Gender { get; set; }

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public int Creator { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? Updator { get; set; }
}

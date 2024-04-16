using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class MemberInfo
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool IsDelete { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}

using System;
using System.Collections.Generic;

namespace CoffeeShop.Model.Entities;

public partial class User
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public DateTime CreateDate { get; set; }
    public int Creator {  get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? Updator { get; set; }
}

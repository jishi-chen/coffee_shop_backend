using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Model.ViewModels
{
    public class UserIndexViewModel
    {
        public int UserId { get; set; }
        public string TenantName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsEnabled { get; set; }
    }
}

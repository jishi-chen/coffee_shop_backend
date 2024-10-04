using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.Interface
{
    public interface ITenantService
    {
        IEnumerable<Tenant> GetAll();
        IEnumerable<Tenant> GetAll(string searchString);
        Tenant GetById(int id);
        TenantFormViewModel GetFormViewModel(int? id);

        void UpdateTenant(TenantFormViewModel model);
        bool DeleteTenant(int? id);
    }
}

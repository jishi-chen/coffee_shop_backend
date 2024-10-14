using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Service.Interface
{
    public interface ITenantService
    {
        IEnumerable<Tenant> GetAll(string? searchString);
        TenantFormViewModel GetFormViewModel(int? id);

        void UpdateTenant(TenantFormViewModel model);
        bool DeleteTenant(int? id);
        IEnumerable<SelectListItem> GetTenantList(int? tenantId);
    }
}

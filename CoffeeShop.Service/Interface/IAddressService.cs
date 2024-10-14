using CoffeeShop.Model.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Service.Interface
{
    public interface IAddressService
    {
        List<SelectListItem> GetAddressCityList(string? cityId);
        List<SelectListItem> GetAddressAreaList(string? cityId, string? areaId);
        AddressArea? GetAddressAreaById(int id);
        string? GetZipCodeByAreaId(int areaId);
    }
}

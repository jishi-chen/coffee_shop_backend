using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoffeeShop.Utility.Helpers
{
    public class AddressHelper
    {
        protected static CoffeeShopContext _db;
        public AddressHelper(CoffeeShopContext coffeeShopContext) {
            _db = coffeeShopContext;
        }

        public List<SelectListItem> GetAddressCityList(string? cityId)
        {
            List<SelectListItem> cityList = new List<SelectListItem>();
            List<AddressCity> cityModel = _db.AddressCities.Select(x => x).ToList();
            foreach (var c in cityModel)
            {
                cityList.Add(new SelectListItem()
                {
                    Text = c.CityName,
                    Value = c.Id.ToString(),
                    Selected = cityId == c.Id.ToString(),
                });
            }
            return cityList;
        }

        public List<SelectListItem> GetAddressAreaList(string? cityId, string? areaId)
        {
            List<SelectListItem> areaList = new List<SelectListItem>();
            List<AddressArea> areaModel = _db.AddressAreas.Where(x => x.CityId.ToString() == cityId).ToList();
            foreach (var c in areaModel)
            {
                areaList.Add(new SelectListItem()
                {
                    Text = c.AreaName,
                    Value = c.Id.ToString(),
                    Selected = areaId == c.Id.ToString(),
                });
            }
            return areaList;
        }
    }
}

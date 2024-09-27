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

        public List<SelectListItem> GetAddressCityList(string? cityName)
        {
            List<SelectListItem> cityList = new List<SelectListItem>();
            List<AddressCity> cityModel = _db.AddressCities.Select(x => x).ToList();
            foreach (var c in cityModel)
            {
                cityList.Add(new SelectListItem()
                {
                    Text = c.CityName,
                    Value = c.CityName,
                    Selected = cityName == c.CityName,
                });
            }
            return cityList;
        }
    }
}

using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class ApplicationController : BaseController
    {
        private HttpContext? _context;

        public ApplicationController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            var model = new BasicData();
            List<SelectListItem> cityList = new List<SelectListItem>();
            List<SelectListItem> areaList = new List<SelectListItem>();
            List<AddressCity> cityModel = _db.AddressCities.Select(x => x).ToList();
            foreach (var c in cityModel)
            {
                cityList.Add(new SelectListItem()
                {
                    Text = c.CityName,
                    Value = c.CityName,
                    Selected = model.Address?.City == c.CityName,
                });
            }
            ViewBag.CityList = cityList;
            ViewBag.AreaList = areaList;

            return View(model);
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(BasicData model)
        {

            return View();
        }

        [Route("Settings")]
        public IActionResult Settings()
        {

            return View();
        }

        [HttpPost]
        [Route("Address")]
        public JsonResult Address(string cityName, string areaName)
        {
            if (!string.IsNullOrEmpty(areaName) && !string.IsNullOrEmpty(cityName))
            {
                var cityId = _db?.AddressCities.FirstOrDefault(x => x.CityName == cityName)?.Id;
                var area = _db?.AddressAreas.FirstOrDefault(x => x.AreaName == areaName && x.CityId == cityId);
                return this.Json(area?.ZipCode);
            }
            else if (!string.IsNullOrEmpty(cityName))
            {
                List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
                var cityId = _db?.AddressCities.FirstOrDefault(x => x.CityName == cityName)?.Id;
                var areas = _db?.AddressAreas.Where(x => x.CityId == cityId);
                if (areas.Any())
                {
                    foreach (var area in areas)
                    {
                        items.Add(new KeyValuePair<string, string>(area.AreaName, area.AreaName));
                    }
                }
                return this.Json(items);
            }
            return this.Json(string.Empty);
        }
    }

    public class BasicData
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? IdentityString { get; set; }
        public Address Address { get; set; } = new Address();
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public List<SelectListItem> GenderOption { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("男", "0"){ Selected = true },
            new SelectListItem("女", "1"),
        };
        public DateTime? DateTimeObject { get; set; } = DateTime.Now;
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? AddressField { get; set; }
    }

}

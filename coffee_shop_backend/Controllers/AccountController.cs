using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private HttpContext? _context;

        public AccountController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var account = _db.Accounts.Where(x => x.IdentityString == model.IdentityString).FirstOrDefault();
            if (account != null && account.Password == model.Password)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.IdentityString),
                new Claim(ClaimTypes.Role, "Admin")  // 假設使用者是 Admin 角色
            };
                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.
                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Index");
            }
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
            DataCheckUtility check = new DataCheckUtility();
            string errorMsg = string.Empty;
            if (!check.CheckNotNULLAndLength(model.Name, 20, out errorMsg))
                ModelState.AddModelError("Name", "姓名" + errorMsg);
            if (!check.CheckNotNULLAndLength(model.IdentityString, 20, out errorMsg))
                ModelState.AddModelError("IdentityString", "身分證字號錯誤" + errorMsg);
            if (!check.IsAlphaNumeric(model.Password))
                ModelState.AddModelError("Password", "密碼格式錯誤");
            if (!check.IsCellPhone(model.Phone))
                ModelState.AddModelError("Phone", "電話格式錯誤");
            if (!check.IsEmail(model.Email))
                ModelState.AddModelError("Email", "信箱格式錯誤");

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

            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<BasicData, Account>());
                Account account = config.CreateMapper().Map<Account>(model);
                account.CreateDate = DateTime.Now;
                _db.Accounts.Add(account);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
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
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }

    public class BasicData
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? IdentityString { get; set; }
        public Address Address { get; set; } = new Address();
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gender { get; set; }
        public List<SelectListItem> GenderOption { get; set; } = new List<SelectListItem>()
        {
            new SelectListItem("男", "0"){ Selected = true },
            new SelectListItem("女", "1"),
        };
        public string? Description { get; set; }
    }

    public class Address
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
        public string? AddressField { get; set; }
    }

}

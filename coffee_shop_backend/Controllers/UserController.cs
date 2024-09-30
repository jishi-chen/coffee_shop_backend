using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class UserController : BaseController
    {
        private HttpContext? _context;
        private AddressHelper _addressHelper = new AddressHelper(_db);

        public UserController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [HttpGet]
        [Route("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            string validCode = _context.Request.Form[ValidateCodeHelper.ValidateCodePostName];
            ValidateCodeHelper.Validate(validCode, ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            //檢查驗證碼
            if (!ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]))
            {
                ViewBag.IsValidated = false;
                return View(model);
            }

            var user = _db.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();
            var passwordHasher = new PasswordHasher<UserLoginViewModel>();
            //密碼解密
            if (user != null && passwordHasher.VerifyHashedPassword(model, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")  // 假設使用者是 Admin 角色
            };
                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Index");
            }
            SetAlertMsg("登入失敗");
            return View(model);
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register(int? id)
        {
            var model = new UserRegisterViewModel();
            string cityId = string.Empty, areaId = string.Empty;
            if (id.HasValue)
            {
                var user = _db.Users.Find(id.Value);
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CoffeeShop.Model.Entities.User, UserRegisterViewModel>().ForMember(dest => dest.Address, opt => opt.Ignore()));
                var mapper = config.CreateMapper();
                mapper.Map(user, model);
                model.Password = "";
                model.ConfirmPassword = "";
                if (user.AddressId.HasValue)
                {
                    var area = _db.AddressAreas.Find(user.AddressId.Value);
                    cityId = area.CityId.ToString();
                    areaId = area.Id.ToString();
                    model.Address.PostalCode = area.ZipCode;
                    model.Address.AddressField = user.Address;
                }
            }
            ViewBag.CityList = _addressHelper.GetAddressCityList(cityId);
            ViewBag.AreaList = _addressHelper.GetAddressAreaList(cityId, areaId);
            ViewBag.TenantList = GetTenantList(model.TenantId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public IActionResult Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                DataCheckUtility check = new DataCheckUtility();
                string errorMsg = string.Empty;
                if (!check.CheckNotNULLAndLength(model.UserName, 20, out errorMsg))
                    ModelState.AddModelError("Name", "姓名" + errorMsg);
                if (model.Password.Length > 0 && !check.IsAlphaNumeric(model.Password))
                    ModelState.AddModelError("Password", "密碼格式錯誤");
                if (!check.IsEmail(model.Email))
                    ModelState.AddModelError("Email", "信箱格式錯誤");
                if (model.Password.Length > 0 && model.ConfirmPassword.Length > 0 && model.Password != model.ConfirmPassword)
                    ModelState.AddModelError("ConfirmPassword", "確認密碼與密碼必須相同");

                //密碼加密
                var passwordHasher = new PasswordHasher<UserRegisterViewModel>();

                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<UserRegisterViewModel, User>());
                var mapper = config.CreateMapper();

                var user = new User();
                if (!model.UserId.HasValue)
                {
                    user = mapper.Map<User>(model);
                    user.PasswordHash = passwordHasher.HashPassword(model, model.Password);
                    user.AddressId = int.TryParse(model.Address.Region, out int addressId) ? addressId : -1;
                    user.Address = model.Address.AddressField;
                    user.CreateDate = DateTime.Now;
                    user.Creator = GetCurrentLoginId();
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    SetAlertMsg("建立完成");
                    return RedirectToAction("Index");
                }
                else
                {
                    user = _db.Users.Find(model.UserId.Value);
                    if (user != null)
                    {
                        mapper.Map(model, user);
                        user.AddressId = int.TryParse(model.Address.Region, out int addressId) ? addressId : -1;
                        user.Address = model.Address.AddressField;
                        user.UpdateDate = DateTime.Now;
                        user.Updator = GetCurrentLoginId();
                        _db.SaveChanges();
                        SetAlertMsg("操作完成");
                        return RedirectToAction("Index");
                    }
                }
            }

            ViewBag.CityList = _addressHelper.GetAddressCityList(model.Address?.City);
            ViewBag.AreaList = _addressHelper.GetAddressAreaList(model.Address?.City, model.Address?.Region); ;
            ViewBag.TenantList = GetTenantList(model.TenantId);
            SetAlertMsg("操作失敗");
            return View(model);
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var result = from user in _db.Users
                         join tenant in _db.Tenants on user.TenantId equals tenant.TenantId
                         select new UserIndexViewModel
                         {
                             UserId = user.UserId,
                             UserName = user.UserName,
                             TenantName = tenant.TenantName,
                             Email = user.Email,
                             IsEnabled = true
                         };
            var model = result.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            var result = from user in _db.Users
                         join tenant in _db.Tenants on user.TenantId equals tenant.TenantId
                         select new UserIndexViewModel
                         {
                             UserId = user.UserId,
                             UserName = user.UserName,
                             TenantName = tenant.TenantName,
                             Email = user.Email,
                             IsEnabled = true
                         };
            var model = result.ToList();
            if (!string.IsNullOrEmpty(searchString))
                model = model.Where(x => x.UserName.Contains(searchString) || x.TenantName.Contains(searchString)).ToList();
            return View(model);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            SetAlertMsg("已成功登出");
            return RedirectToAction("Index");
        }

        public List<SelectListItem> GetTenantList(int? TenantId)
        {
            List<SelectListItem> tenantList = new List<SelectListItem>();
            List<Tenant> list = _db.Tenants.Where(x => x.IsEnabled == true).ToList();
            foreach (var c in list)
            {
                tenantList.Add(new SelectListItem()
                {
                    Text = c.TenantName,
                    Value = c.TenantId.ToString(),
                    Selected = TenantId.HasValue? TenantId.Value == c.TenantId : false,
                });
            }
            return tenantList;
        }
    }



}

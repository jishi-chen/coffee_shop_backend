using CoffeeShop.Model.ViewModels;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class UserController : BaseController
    {
        private readonly HttpContext? _context;
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private readonly ITenantService _tenantService;

        public UserController(IUserService userService, IHttpContextAccessor accessor, IAddressService addressService, ITenantService tenantService) : base(accessor)
        {
            _context = accessor.HttpContext;
            _userService = userService;
            _addressService = addressService;
            _tenantService = tenantService;
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
            string? validCode = _context?.Request.Form[ValidateCodeHelper.ValidateCodePostName];
            ValidateCodeHelper.Validate(validCode, ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            //檢查驗證碼
            if (!ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]))
            {
                ViewBag.IsValidated = false;
                SetAlertMsg("驗證碼錯誤");
                return View(model);
            }

            var user = _userService.CheckPassword(model);
            if (user != null)
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
        public IActionResult Register(int? userId)
        {
            string cityId = string.Empty, areaId = string.Empty;
            var model = _userService.GetFormViewModel(userId, ref cityId, ref areaId);
            ViewBag.CityList = _addressService.GetAddressCityList(cityId);
            ViewBag.AreaList = _addressService.GetAddressAreaList(cityId, areaId);
            ViewBag.TenantList = _tenantService.GetTenantList(model.TenantId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public IActionResult Register(UserRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                _userService.UpdateUser(model);
                SetAlertMsg("操作完成");
                return RedirectToAction("Index");
            }
            ViewBag.CityList = _addressService.GetAddressCityList(model.Address?.City);
            ViewBag.AreaList = _addressService.GetAddressAreaList(model.Address?.City, model.Address?.Region); ;
            ViewBag.TenantList = _tenantService.GetTenantList(model.TenantId);
            SetAlertMsg("操作失敗");
            return View(model);
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View(_userService.GetIndexViewModel(null));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Index")]
        public IActionResult Index(string searchString, PaginationModel pagination)
        {
            ViewBag.SearchString = searchString;
            return View(GetPaginatedList(_userService.GetIndexViewModel(searchString), pagination.PageIndex));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("SetEnabled")]
        public IActionResult SetEnabled(int userId)
        {
            _userService.SetEnabled(userId);
            SetAlertMsg("操作完成");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            SetAlertMsg("已成功登出");
            return RedirectToAction("Index");
        }
    }
}

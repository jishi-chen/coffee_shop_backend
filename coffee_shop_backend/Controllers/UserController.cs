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
    public class UserController : BaseController
    {
        private HttpContext? _context;
        private AddressHelper _addressHelper = new AddressHelper(_db);

        public UserController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            UserLoginViewModel model = new UserLoginViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
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
                new Claim(ClaimTypes.Name, model.UserName),
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
            return View(model);
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            var model = new UserRegisterViewModel();
            ViewBag.CityList = _addressHelper.GetAddressCityList("");
            ViewBag.AreaList = new List<SelectListItem>();
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
                if (!check.IsAlphaNumeric(model.Password))
                    ModelState.AddModelError("Password", "密碼格式錯誤");
                if (!check.IsEmail(model.Email))
                    ModelState.AddModelError("Email", "信箱格式錯誤");

                //密碼加密
                var passwordHasher = new PasswordHasher<UserRegisterViewModel>();
                model.Password = passwordHasher.HashPassword(model, model.Password);

                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<UserRegisterViewModel, User>());
                User user = config.CreateMapper().Map<User>(model);
                user.CreateDate = DateTime.Now;
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            ViewBag.CityList = _addressHelper.GetAddressCityList(model.Address?.City);
            ViewBag.AreaList = new List<SelectListItem>();
            return View(model);
        }

        [Route("Settings")]
        public IActionResult Settings()
        {

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }



}

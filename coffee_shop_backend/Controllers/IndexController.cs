using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IndexController : BaseController
    {
        private HttpContext? _context;

        public IndexController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 在這裡進行一些 Action 執行前的操作

            base.OnActionExecuting(context);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            // 寫入 cookie
            _context.Response.Cookies.Append("myCookie", "myValue", new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                Path = "/",
                Secure = true
            });
            var myCookieValue = _context.Request.Cookies["myCookie"];
            _context.Response.Cookies.Delete("myCookie");
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult Index4()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index2(string name)
        {
            string validCode = _context.Request.Form[ValidateCodeHelper.ValidateCodePostName];
            ValidateCodeHelper.Validate(validCode, ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            //檢查驗證碼
            if (!ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]))
            {
                ViewBag.ModifySuccess = "驗證碼錯誤";
                return View();
            }

            ValidateCodeHelper.RemoveResult(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            ViewBag.ModifySuccess = "驗證碼正確";
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }

}

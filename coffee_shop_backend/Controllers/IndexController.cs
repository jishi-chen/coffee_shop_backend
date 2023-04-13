using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    public class IndexController : BaseController
    {
        private HttpContext? _context;

        public IndexController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
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
    }
}

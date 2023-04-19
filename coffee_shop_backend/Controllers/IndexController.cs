using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace coffee_shop_backend.Controllers
{
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
    public class BasicData
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string IdentityString { get; set; }
        public Address Address { get; set; }

    }

    public class Address
    {
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string AddressField { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    public class TestController : BaseController
    {
        private HttpContext? _context;

        public TestController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        public IActionResult Index()
        {
            var myCookieValue = _context.Request.Cookies["myCookie"];
            return View();
        }
    }
}

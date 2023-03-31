using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    public class IndexController : BaseController
    {
        public IndexController(IHttpContextAccessor accessor) : base(accessor)
        {
           
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index2()
        {
            return View();
        }
    }
}

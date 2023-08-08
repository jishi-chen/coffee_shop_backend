using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

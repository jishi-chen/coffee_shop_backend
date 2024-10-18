using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using coffee_shop_backend.Controllers;

namespace coffee_shop_backend.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = "AdminPolicy")]
    public class ModuleController : BaseController
    {
        private readonly HttpContext? _context;

        public ModuleController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

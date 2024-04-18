using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace coffee_shop_backend.Controllers
{
    [AllowAnonymous]
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

        public IActionResult Index()
        {
            return View();
        }
    }

}

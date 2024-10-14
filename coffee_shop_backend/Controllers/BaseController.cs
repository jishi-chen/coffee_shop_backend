using CoffeeShop.Model;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace coffee_shop_backend.Controllers
{
    public class BaseController : Controller
    {
        public static IHttpContextAccessor HttpContextAccessor;
        protected static IConfiguration _config => HttpContextAccessor.HttpContext?.RequestServices.GetService<IConfiguration>()!;
        protected static IHostEnvironment _host => HttpContextAccessor.HttpContext?.RequestServices.GetService<IHostEnvironment>()!;
        protected static IHttpClientFactory _httpClientFactory => HttpContextAccessor.HttpContext?.RequestServices.GetService<IHttpClientFactory>()!;
        public BaseController(IHttpContextAccessor accessor)
        {
            HttpContextAccessor = accessor;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public int GetCurrentLoginId()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                return int.TryParse(userIdClaim?.Value, out int userId) ? userId : 0;
            }
            return 0;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetAlertMsg(string msg)
        {
            TempData["AlertMsg"] = msg;
        }
    }
}

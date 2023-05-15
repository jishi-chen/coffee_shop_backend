using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    public class BaseController : Controller
    {
        public static IHttpContextAccessor HttpContextAccessor;
        protected static IConfiguration _config => HttpContextAccessor.HttpContext?.RequestServices.GetService<IConfiguration>()!;
        protected static IHostEnvironment _host => HttpContextAccessor.HttpContext?.RequestServices.GetService<IHostEnvironment>()!;
        protected static IHttpClientFactory _httpClientFactory => HttpContextAccessor.HttpContext?.RequestServices.GetService<IHttpClientFactory>()!;
        protected static ModelContext? _db => HttpContextAccessor.HttpContext?.RequestServices.GetService<ModelContext>();
        public BaseController(IHttpContextAccessor accessor)
        {
            HttpContextAccessor = accessor;
        }
    }
}

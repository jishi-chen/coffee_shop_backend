using CoffeeShop.Model.ViewModels;
using Microsoft.AspNetCore.Mvc;


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
        protected void SetAlertMsg(string msg)
        {
            TempData["AlertMsg"] = msg;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected IEnumerable<T> GetPaginatedList<T>(IEnumerable<T> allItems, int pageIndex)
        {
            int pageSize = 2;
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            var count = allItems.Count(); // 總筆數
            PaginatedList paginatedList = new PaginatedList(count, pageIndex, pageSize);
            ViewBag.PaginationModel = paginatedList.PaginationModel;
            return allItems = allItems.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}

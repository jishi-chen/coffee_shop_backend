using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.ViewComponents
{
    public class Footer : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            ViewBag.Message = $"Hello, {name}";
            return View("_Footer");
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;

namespace coffee_shop_backend.Filters
{
    public class CustomAttribute : ActionFilterAttribute
    {
        private const string CookieName = "MemberAuthentication";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var authCookie = filterContext.HttpContext.Request.Cookies[CookieName];
        }
    }
}

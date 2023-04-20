namespace coffee_shop_backend.Middleware
{
    public static class CustomMiddlewareExtensions
    {
        public static void UseCustom(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware>();
        }
        public static void UseCustom2(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomMiddleware2>();
        }
    }
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            await context.Response.WriteAsync("Message 1 \r\n");
        }
    }

    public class CustomMiddleware2
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware2(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
            await context.Response.WriteAsync("Message 2 \r\n");
        }
    }
}

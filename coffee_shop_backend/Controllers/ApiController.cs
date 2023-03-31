using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : BaseController
    {
        public ApiController(IHttpContextAccessor accessor) : base(accessor)
        {

        }

        [HttpGet]
        [Route("test")]

        public IActionResult Index()
        {

            return this.Ok(new
            {
                Version = 1.0,
                Name = "1.0"
            });
        }
    }
}

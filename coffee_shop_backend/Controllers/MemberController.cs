using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : BaseController
    {
        public MemberController(IHttpContextAccessor accessor) : base(accessor)
        {


        }
        [HttpGet]
        [Route("get")]
        public IActionResult Get()
        {
            return this.Ok(new
            {
                Version = 1.0,
                Name = "1.0"
            });
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login()
        {
            return this.Ok(new
            {
                Version = 1.0,
                Name = "1.0"
            });
        }

    }
}

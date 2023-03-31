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
        public IActionResult Login([FromBody] DataModel data)
        {
            try
            {
                string email = data.email;

                return Ok(new { message = "成功接收到資料！" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "發生錯誤：" + ex.Message });
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register()
        {

            return this.Ok(new
            {
                Version = 1.0,
                Name = "1.0"
            });
        }

        public class DataModel
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}

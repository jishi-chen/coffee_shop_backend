using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace coffee_shop_backend.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route("shop")]

        public IActionResult Shop()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            // 解碼JWT Token
            var decodedToken = tokenHandler.ReadJwtToken(token);

            // 獲取帳號資訊
            var user = decodedToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Aud)?.Value;
            MemberInfo? info = _db?.MemberInfos.FirstOrDefault(x => x.UserName == user);

            return this.Ok(new
            {
                name = info?.UserName,
                price = 1000
            });
        }
    }
}

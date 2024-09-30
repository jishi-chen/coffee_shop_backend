using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Asp.Versioning;
using CoffeeShop.Model.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace coffee_shop_backend.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : BaseController
    {
        private HttpContext? _context;

        public MemberController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginDataModel data)
        {
            try
            {
                Member? info = _db?.Members.FirstOrDefault(x => x.UserName == data.username && x.Password == data.password);
                if (info != null)
                {
                    var secret = _config["AccessSecret"]; // 密鑰
                    var issuer = _config["Issuer"]; // 發行者
                    var audience = info.UserName; // 使用者
                    var expires = 60; // JWT 有效期，單位為分鐘

                    // 會員登入成功
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, info.UserName),
                        new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(info)),
                        new Claim(ClaimTypes.Role, "Member"),
                    };
                    var token = JwtHelper.GenerateToken(secret, issuer, audience, claims, expires);

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTime.Now.AddHours(1),
                        IsPersistent = true
                    };
                    _context?.SignInAsync(JwtBearerDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                    return Ok(new
                    {
                        status = "success",
                        message = "會員登入成功",
                        token = token
                    });
                }
                else
                {
                    // 會員登入失敗
                    return Unauthorized(new
                    {
                        status = "error",
                        message = "帳號或密碼錯誤"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = "發生錯誤：" + ex.Message });
            }
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] RegisterDataModel data)
        {
            try
            {
                Member model = new Member()
                {
                    Id = Guid.NewGuid(),
                    UserName = data.username,
                    Email = data.email,
                    Password = data.password,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                };
                _db?.Members.Add(model);
                _db?.SaveChanges();

                return Ok(new { message = "成功接收到資料！" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "發生錯誤：" + ex.Message });
            }
        }

        [HttpPost]
        [Route("Info")]
        public IActionResult Info()
        {

            try
            {
                return Ok(new { message = "成功接收到資料！" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "發生錯誤：" + ex.Message });
            }
        }

        // 定義一個用於生成 JWT 的類
        public static class JwtHelper
        {
            public static string GenerateToken(string secret, string issuer, string audience, List<Claim> claims, int expires)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(expires),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

        public class LoginDataModel
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class RegisterDataModel
        {
            public string username { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}

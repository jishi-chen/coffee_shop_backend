using Asp.Versioning;
using CoffeeShop.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace coffee_shop_backend.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    [Route("api")]
    public class ApiController : BaseController
    {
        public ApiController(IHttpContextAccessor accessor) : base(accessor)
        {

        }

        [HttpGet]
        [Route("test")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]

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
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Address")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public JsonResult Address(string cityName, string areaName)
        {
            if (!string.IsNullOrEmpty(areaName) && !string.IsNullOrEmpty(cityName))
            {
                var cityId = _db?.AddressCities.FirstOrDefault(x => x.CityName == cityName)?.Id;
                var area = _db?.AddressAreas.FirstOrDefault(x => x.AreaName == areaName && x.CityId == cityId);
                Response.StatusCode = 200;
                return this.Json(area?.ZipCode);
            }
            else if (!string.IsNullOrEmpty(cityName))
            {
                List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
                var cityId = _db?.AddressCities.FirstOrDefault(x => x.CityName == cityName)?.Id;
                var areas = _db?.AddressAreas.Where(x => x.CityId == cityId);
                if (areas.Any())
                {
                    foreach (var area in areas)
                    {
                        items.Add(new KeyValuePair<string, string>(area.AreaName, area.AreaName));
                    }
                }
                Response.StatusCode = 200;
                return this.Json(items);
            }
            else
            {
                Response.StatusCode = 500;
                return this.Json(string.Empty);
            }
        }
    }
}

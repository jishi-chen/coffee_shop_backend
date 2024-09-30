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
            Member? info = _db?.Members.FirstOrDefault(x => x.UserName == user);

            return this.Ok(new
            {
                name = info?.UserName,
                price = 1000
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Address")]
        public IActionResult Address([FromBody] AddressModel model)
        {
            if (!string.IsNullOrEmpty(model.areaId) && !string.IsNullOrEmpty(model.cityId))
            {
                var area = _db?.AddressAreas.FirstOrDefault(x => x.Id.ToString() == model.areaId && x.CityId.ToString() == model.cityId);
                Response.StatusCode = 200;
                return this.Json(area?.ZipCode);
            }
            else if (!string.IsNullOrEmpty(model.cityId))
            {
                List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();
                var areas = _db?.AddressAreas.Where(x => x.CityId.ToString() == model.cityId);
                if (areas.Any())
                {
                    foreach (var area in areas)
                    {
                        items.Add(new KeyValuePair<string, string>(area.Id.ToString(), area.AreaName));
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

        public class AddressModel
        {
            public string cityId { get; set; } = null!;
            public string areaId { get; set; } = null!;
        }
    }
}

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CoffeeShop.Utility.Helpers;

namespace coffee_shop_backend.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : BaseController
    {
        private HttpContext? _context;
        private IMemoryCache _memoryCache { get; set; }
        private readonly IDistributedCache _cache;

        public TestController(IHttpContextAccessor accessor, IMemoryCache memoryCache, IDistributedCache cache) : base(accessor)
        {
            _context = accessor.HttpContext;
            _memoryCache = memoryCache;
            _cache = cache;
        }

        public IActionResult Index()
        {
            // 寫入 cookie
            _context.Response.Cookies.Append("myCookie", "myValue", new CookieOptions
            {
                Expires = DateTime.Now.AddHours(1),
                Path = "/",
                Secure = true
            });
            var myCookieValue = _context.Request.Cookies["myCookie"];
            _context.Response.Cookies.Delete("myCookie");
            return View();
        }

        public IActionResult Index2()
        {
            List<string> list = new List<string>();
            int a = 5;
            int b = 0;
            var add = (int value) => a++;
            add += (int value) => value++;
            b = add(a);
            return View();
        }
        public IActionResult Index3()
        {
            return View();
        }
        public IActionResult Index4()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index2(string name)
        {
            string validCode = _context.Request.Form[ValidateCodeHelper.ValidateCodePostName];
            ValidateCodeHelper.Validate(validCode, ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            //檢查驗證碼
            if (!ValidateCodeHelper.IsSuccess(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]))
            {
                ViewBag.ModifySuccess = "驗證碼錯誤";
                return View();
            }

            ValidateCodeHelper.RemoveResult(ConstString.FrontEndSessionName + ControllerContext.RouteData.Values["controller"]);
            ViewBag.ModifySuccess = "驗證碼正確";
            return View();
        }

        public IActionResult Index5()
        {
            // 透過 SqlClient 取得 SQL Server 產生的 JSON 資料

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"(localhost\\SQLEXPRESS)";
            //builder.UserID = "<your_username>";
            //builder.Password = "<your_password>";
            builder.InitialCatalog = "CoffeeShop";
            builder.IntegratedSecurity = true;
            builder.MultipleActiveResultSets = true;
            builder.TrustServerCertificate = true;
            var sql = "SELECT * FROM AddressCity FOR JSON PATH";
            //using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            //{
            //    using (var cmd = new SqlCommand(sql, conn))
            //    {
            //        var jsonResult = new StringBuilder();
            //        conn.Open();

            //        var reader = cmd.ExecuteReader();
            //        if (!reader.HasRows)
            //        {
            //            jsonResult.Append("[]");
            //        }
            //        else
            //        {
            //            while (reader.Read())
            //            {
            //                jsonResult.Append(reader.GetValue(0).ToString());
            //            }
            //        }
            //        Console.WriteLine(jsonResult.ToString());
            //    };
            //};

            // 漂亮的建立 Query String 查詢字串

            var authUrl = "https://access.line.me/oauth2/v2.1/authorize";
            string url = "";
            //使用 QueryBuilder 類別
            var qb = new QueryBuilder();
            qb.Add("response_type", "code");
            qb.Add("client_id", _config["LINELogin:client_id"]);
            qb.Add("redirect_uri", _config["LINELogin:redirect_uri"]);
            qb.Add("scope", _config["LINELogin:scope"]);
            qb.Add("state", "");
            url = $"{authUrl}{qb.ToQueryString().Value}";

            //使用 QueryHelpers 類別
            var query = new Dictionary<string, string>
            {
                ["response_type"] = "code",
                ["client_id"] = _config["LINELogin:client_id"],
                ["state"] = "",
                ["scope"] = _config["LINELogin:scope"],
                ["redirect_uri"] = _config["LINELogin:redirect_uri"],
            };
            url = QueryHelpers.AddQueryString(authUrl, query);

            //用 .NET 解析完整的網址
            var authUrl2 = "https://access.line.me/oauth2/v2.1/authorize?response_type=code&client_id=111111&state=222&scope=profile%20openid%20email&redirect_uri=https%3A%2F%2Flocalhost%3A7133%2Fcallback";
            var uri = new Uri(authUrl2);
            var uriPath = uri.GetLeftPart(UriPartial.Path); // https://access.line.me/oauth2/v2.1/authorize
            var uriQuery = uri.GetLeftPart(UriPartial.Query);  //問號後面的
            var uriQuery2 = uri.Query;
            var uriScheme = uri.GetLeftPart(UriPartial.Scheme); // https://              
            var uriAuthority = uri.GetLeftPart(UriPartial.Authority); // https://access.line.me

            var queryString = QueryHelpers.ParseQuery(uri.Query);
            var client_id = queryString["client_id"].ToString();


            // 使用 Serilog 進行結構化記錄
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // 偵測用戶端已斷線並自動取消非同步方法執行
            //_db.SaveChangesAsync(_context.RequestAborted);
            if (_context.RequestAborted.IsCancellationRequested)
            {
                // 如果你想在最後檢查用戶端是否斷線，如果斷線可以在這裡進行一些髒資料清理動作！
            }
            return View();
        }

        public IActionResult Index6()
        {
            //Memory Cache
            DateTime cacheEntry;

            // 嘗試取得指定的Cache
            if (!_memoryCache.TryGetValue("CachKey", out cacheEntry))
            {
                // 指定的Cache不存在，所以給予一個新的值
                cacheEntry = DateTime.Now;
                // 設定Cache選項
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // 設定Cache保存時間，如果有存取到就會刷新保存時間
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                // 把資料除存進Cache中
                _memoryCache.Set("CachKey", cacheEntry, cacheEntryOptions);
            }

            Dictionary<string, string> model = new Dictionary<string, string>()
            {
                {"現在時間", DateTime.Now.ToString()},
                {"快取內容", cacheEntry.ToString()}
            };
            ViewBag.MemoryCache = model;


            //Redis 分散式快取
            string key = "test";
            string result = "Sample Data";
            // 檢查快取中是否有指定鍵的值
            var cachedValue = _cache.GetString(key);
            if (cachedValue != null)
            {
                result = cachedValue;
            }
            else
            {
                // 若快取中沒有值，則進行資料查詢或計算，並將結果存入快取
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)  // 設定快取過期時間
                };
                _cache.SetString(key, result, options);
            }
            ViewBag.DistributedCache = result;
            return View();
        }

        public IActionResult Index7()
        {
            return View();
        }

        [Route("[controller]/GetFile")]
        public IActionResult GetFile()
        {
            int id = 1;
            string path = _host.ContentRootPath;
            if (id <= 0)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    errorno = 1,
                    message = "id must larger than zero."
                });
            }
            return File("image/testImg.PNG", "image/jpeg"); //以 web root 路徑為起點 (wwwroot)
        }

        [Route("[controller]/Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                FileUtility fileUtility = new FileUtility()
                {
                    allowedExtensions = ".jpg,.zip"
                };
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                using (Stream stream = file.OpenReadStream())
                {
                    string errMsg = fileUtility.CheckIsUploadFilesValid(stream, Path.GetExtension(fileName));
                    if (!string.IsNullOrEmpty(errMsg))
                    {
                        return BadRequest("檔案格式錯誤");
                    }
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    
                    await file.CopyToAsync(stream);
                }
                return Ok();
            }
            return BadRequest("No file was uploaded.");
        }

    }
}

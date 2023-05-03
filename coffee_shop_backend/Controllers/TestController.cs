using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Serilog;
using Serilog.Events;
using System.Net;
using System.Text;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : BaseController
    {
        private HttpContext? _context;

        public TestController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }

        [Route("test")]
        public IActionResult Index()
        {
            // 透過 SqlClient 取得 SQL Server 產生的 JSON 資料

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = @"(local)";
            //builder.UserID = "<your_username>";
            //builder.Password = "<your_password>";
            builder.InitialCatalog = "CoffeeShop";
            builder.IntegratedSecurity = true;
            builder.MultipleActiveResultSets = true;
            builder.TrustServerCertificate = true;
            var sql = "SELECT * FROM AddressCity FOR JSON PATH";
            using (SqlConnection conn = new SqlConnection(builder.ConnectionString))
            {
                using (var cmd = new SqlCommand(sql, conn))
                {
                    var jsonResult = new StringBuilder();
                    conn.Open();

                    var reader = cmd.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                    }
                    Console.WriteLine(jsonResult.ToString());
                };
            };

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

            List<int>  result = highest_biPrimefac(2, 3, 50);
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

        public List<int> highest_biPrimefac(int a, int b, int max)
        {
            if (a > b || a < 1 || b < 1) return null;

            List<int> re = new List<int>();
            int temp = 0;
            int temp2 = 0;
            int result = 0;
            int pow = 1;
            int pow2 = 1;
            while(max > result)
            {
                temp = (int)Math.Pow(a, pow);
                temp2 = (int)Math.Pow(b, pow2);
                if (temp * temp2 > max)
                {
                    break;
                }
                result = result > (temp * temp2) ? result : (temp * temp2);
                if (temp >= temp2)
                    pow2++;
                else if (temp < temp2)
                    pow++;
            }
            re.Add(pow);
            re.Add(pow2);
            re.Add(result);
            return re;
        }

    }
}

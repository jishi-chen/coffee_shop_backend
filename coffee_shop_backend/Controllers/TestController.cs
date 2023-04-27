using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;

namespace coffee_shop_backend.Controllers
{
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

            

            List<int>  result = highest_biPrimefac(2, 3, 50);
            return View();
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

using coffee_shop_backend.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using coffee_shop_backend.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace coffee_shop_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddViewLocalization()  //為了在 View 中使用多國語言
                .AddDataAnnotationsLocalization()  //為了在 Model 中使用多國語言;
                .AddRazorRuntimeCompilation();  //啟用 Razor 執行階段編譯

            services.AddHttpContextAccessor();
            // Register the Swagger Generator service.
            // This service is responsible for genrating Swagger Documents.
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Document",
                    Description = "API Document For CoffeeShop",
                    Contact = new OpenApiContact
                    {
                        Name = "Willy Chen",
                        Email = string.Empty
                    }
                });
            });
            
            // HttpClient()服務
            services.AddHttpClient("server").ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
                UseCookies = false,
            }); 
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMemoryCache();
            services.AddSession();
            // Entity Framework
            services.AddDbContext<ModelContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //資料保護 cryptographic API
            services.AddDataProtection();

            // Api Version相關資訊
            services.AddApiVersioning(option =>
            {
                //返回響應標頭中支援的版本資訊
                option.ReportApiVersions = true;

                //未提供版本請請時，使用預設版號
                option.AssumeDefaultVersionWhenUnspecified = true;

                //預設api版本號，支援時間或數字版本號 
                option.DefaultApiVersion = new ApiVersion(1, 0);

                //支援MediaType、Header、QueryString 設定版本號；預設為 QueryString、UrlSegment
                option.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader("api-version"),
                    new HeaderApiVersionReader("api-version"),
                    new QueryStringApiVersionReader("api-version"),  // 增加版本的 query string，?api-version=1.1
                    new UrlSegmentApiVersionReader());
            });

            //同源政策相關
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            
            //CSRF token 驗證相關
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                         // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                        //NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                        // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
                        //RoleClaimType = "Member",

                        // 一般我們都會驗證 Issuer
                        ValidateIssuer = true,
                        ValidIssuer = Configuration.GetValue<string>("Issuer"),

                        // 通常不太需要驗證 Audience
                        ValidateAudience = false,
                        //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

                        // 一般我們都會驗證 Token 的有效期間
                        ValidateLifetime = true,

                        // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                        ValidateIssuerSigningKey = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("AccessSecret")))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.Redirect("/login");
                            return Task.CompletedTask;
                        }
                    };
                });
            //OpenID Connect 整合 Google 登入
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.Authority = Configuration["OpenIDConnect:Authority"];
                options.ClientId = Configuration["OpenIDConnect:ClientId"];
                options.ClientSecret = Configuration["OpenIDConnect:ClientSecret"];
                options.ResponseType = "code";

                options.Scope.Add("openid");
                options.Scope.Add("profile");

                // LINE Login 不加入以下這一段設定，將無法走完 OpenID Connect 整個流程，最後會驗不過 JWT Token
                options.Events = new OpenIdConnectEvents()
                {
                    OnAuthorizationCodeReceived = context => {
                        context.TokenEndpointRequest?.SetParameter("id_token_key_type", "JWK");
                        return Task.CompletedTask;
                    }
                };

                options.SaveTokens = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();

            //app.UseHttpsRedirection(); // 強制跳轉https
            //app.UseStatusCodePagesWithRedirects("~/404.html");
            app.UseStaticFiles();

            app.UseRouting(); //遍歷 Controls 找到符合 request 的 action
            app.UseAuthentication();  //先執行驗證再授權
            app.UseAuthorization(); //Controller、Action才能加上 [Authorize] 屬性
            app.UseSession();
            app.UseApiVersioning(); //使用 Api Version Middleware

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            // 在應用程式的根目錄上提供 Swagger UI，請將 RoutePrefix 屬性設為空字串
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Document V1");  // https://localhost:7219/swagger/v1/swagger.json
                //c.RoutePrefix = string.Empty;
                c.RoutePrefix = "swagger";
            });

            // Middleware使用
            //app.UseMiddleware<CustomMiddleware>();
            //app.UseCustom();
            //app.UseCustom2();

            // 註冊路由節點，若request有符合路由節點則執行相應的委派並開始回流
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "Admin",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Index}/{action=Index}/{id?}")
                  .RequireAuthorization(); // <-- 加入這行，這會讓整個網站都需要登入才能用！
                endpoints.MapRazorPages();
            });
        }
    }

    /*
    AddMvcCore()：最輕量。僅有路由解析的必要服務，其他如身分驗證或資料驗證等功能就需再額外引用。
    AddControllers()：次輕量。使用 WebApi首選，比AddMvcCore 多了資料、身分驗證跟cors等等相關的服務。
    AddControllersWithViews()：包含AddControllers 再加上view page的支援，使用標準MVC 服務可選。
    AddRazorPages()：支援 RazorPage 與部分AddControllers 的功能。前端打算用RazorPage，又不需要太多API支援時選用。
    AddMVC()：最完整也最肥的服務。
     */

    /*
     套件使用：
    Swashbuckle.AspNetCore: 
    Swashbuckle.AspNetCore.Swagger：Swagger物件模型和中介軟體，將物件公開 SwaggerDocument 為 JS ON 端點。
    Swashbuckle.AspNetCore.SwaggerGen：Swagger 產生器，可直接從您的路由、控制器和模型建置 SwaggerDocument 物件。 它通常會與 Swagger 端點中介軟體結合，以自動公開 Swagger JS ON。
    Swashbuckle.AspNetCore.SwaggerUI：Swagger UI 工具的內嵌版本。 它會解譯 Swagger JS ON，以建置豐富的可自訂體驗，以描述 Web API 功能。 其中包括公用方法的內建測試載入器。
    Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer / Microsoft.AspNetCore.Mvc.Versioning: Web  API 版本控制
    */
}

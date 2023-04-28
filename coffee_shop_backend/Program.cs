using coffee_shop_backend;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//改寫回讀取Startup設定
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Host.UseSerilog(); // <-- 使用Serilog 加入這一行
var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();


/*
app.Configuration --> IConfiguration
app.Environment --> IWebHostEnvironment
app.Logger --> ILogger
ConfigureServices(IServiceCollection services) 的服務註冊，改用 builder.Services
 */

/*

// 同一網址依 HTTP Method 決定處理邏輯
app.MapGet("/", () => "This is a GET"); 
app.MapPost("/", () => "This is a POST");
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");

// 可一次對映多個 HTTP Method
app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, 
                          () => "This is an options or head request ");
// 由 URL 路徑取得參數
app.MapGet("/users/{userId}/books/{bookId}", 
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
    
// 萬用字元 * 將後方部分視為一個參數
app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");

// 加上參數型別限制
app.MapGet("/todos/{id:int}", (int id) => db.Todos.Find(id));
app.MapGet("/todos/{text}", (string text) => db.Todos.Where(t => t.Text.Contains(text));
app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");

// id 來自 URL 路徑、page 來自 QueryString 或 Header，service 則來自依賴注入
app.MapGet("/{id}", (int id, int page, ISomeService service) => { });
// person 來自 Body JSON
app.MapPost("/", (Person person) => { });
// 明確指定參數來源
app.MapGet("/{id}", ([FromRoute] int id,
                     [FromQuery(Name = "p")] int page,
                     [FromServices] Service service,
                     [FromHeader(Name = "Content-Type")] string contentType) 
                     => {});
// 參數預設為必要，選擇性參數需額外註明
app.MapGet("/list1", (int? pageNumber) => $"Requesting page {pageNumber ?? 1}");
app.MapGet("/list2", (int pageNumber = 1) => $"Requesting page {pageNumber}");

// 操作 HttpRequest、HttpResponse
app.MapGet("/a", (HttpContext context) => context.Response.WriteAsync("Hello World"));
app.MapGet("/b", (HttpRequest request, HttpResponse response) =>
    response.WriteAsync($"Hello World {request.Query["name"]}"));
app.MapGet("/c", async (CancellationToken cancellationToken) => 
    await MakeLongRunningRequestAsync(cancellationToken));
app.MapGet("/d", (ClaimsPrincipal user) => user.Identity.Name);

// 內建回應格式
// 字串內容
app.MapGet("/hello", () => "Hello World"); 
// 傳回JSON
app.MapGet("/hello", () => new { Message = "Hello World" }); 
// 傳回 IResult
app.MapGet("/hello", () => Results.Ok(new { Message = "Hello World" }));
// 指定 StatusCode
app.MapGet("/api/todoitems/{id}", async (int id, TodoDb db) =>
         await db.Todos.FindAsync(id) 
         is Todo todo
         ? Results.Ok(todo) 
         : Results.NotFound())
   .Produces<Todo>(StatusCodes.Status200OK) // 若為 Todo 型別傳 200
   .Produces(StatusCodes.Status404NotFound);
// 傳回 JSON
app.MapGet("/hello", () => Results.Json(new { Message = "Hello World" }));
// 自訂 Status Code
app.MapGet("/405", () => Results.StatusCode(405));
// 傳純文字
app.MapGet("/text", () => Results.Text("This is some text"));
// 傳回 Stream
app.MapGet("/pokemon", async () => 
{
    var stream = await proxyClient.GetStreamAsync("http://consoto/pokedex.json");
    // Proxy the response as JSON
    return Results.Stream(stream, "application/json");
});
// 重新導向
app.MapGet("/old-path", () => Results.Redirect("/new-path"));
// 傳回檔案
app.MapGet("/download", () => Results.File("myfile.text"));

 */
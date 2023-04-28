using coffee_shop_backend;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//��g�^Ū��Startup�]�w
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
builder.Host.UseSerilog(); // <-- �ϥ�Serilog �[�J�o�@��
var app = builder.Build();
startup.Configure(app, app.Environment);
app.Run();


/*
app.Configuration --> IConfiguration
app.Environment --> IWebHostEnvironment
app.Logger --> ILogger
ConfigureServices(IServiceCollection services) ���A�ȵ��U�A��� builder.Services
 */

/*

// �P�@���}�� HTTP Method �M�w�B�z�޿�
app.MapGet("/", () => "This is a GET"); 
app.MapPost("/", () => "This is a POST");
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");

// �i�@����M�h�� HTTP Method
app.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, 
                          () => "This is an options or head request ");
// �� URL ���|���o�Ѽ�
app.MapGet("/users/{userId}/books/{bookId}", 
    (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
    
// �U�Φr�� * �N��賡�������@�ӰѼ�
app.MapGet("/posts/{*rest}", (string rest) => $"Routing to {rest}");

// �[�W�Ѽƫ��O����
app.MapGet("/todos/{id:int}", (int id) => db.Todos.Find(id));
app.MapGet("/todos/{text}", (string text) => db.Todos.Where(t => t.Text.Contains(text));
app.MapGet("/posts/{slug:regex(^[a-z0-9_-]+$)}", (string slug) => $"Post {slug}");

// id �Ӧ� URL ���|�Bpage �Ӧ� QueryString �� Header�Aservice �h�Ӧۨ̿�`�J
app.MapGet("/{id}", (int id, int page, ISomeService service) => { });
// person �Ӧ� Body JSON
app.MapPost("/", (Person person) => { });
// ���T���w�Ѽƨӷ�
app.MapGet("/{id}", ([FromRoute] int id,
                     [FromQuery(Name = "p")] int page,
                     [FromServices] Service service,
                     [FromHeader(Name = "Content-Type")] string contentType) 
                     => {});
// �Ѽƹw�]�����n�A��ܩʰѼƻ��B�~����
app.MapGet("/list1", (int? pageNumber) => $"Requesting page {pageNumber ?? 1}");
app.MapGet("/list2", (int pageNumber = 1) => $"Requesting page {pageNumber}");

// �ާ@ HttpRequest�BHttpResponse
app.MapGet("/a", (HttpContext context) => context.Response.WriteAsync("Hello World"));
app.MapGet("/b", (HttpRequest request, HttpResponse response) =>
    response.WriteAsync($"Hello World {request.Query["name"]}"));
app.MapGet("/c", async (CancellationToken cancellationToken) => 
    await MakeLongRunningRequestAsync(cancellationToken));
app.MapGet("/d", (ClaimsPrincipal user) => user.Identity.Name);

// ���ئ^���榡
// �r�ꤺ�e
app.MapGet("/hello", () => "Hello World"); 
// �Ǧ^JSON
app.MapGet("/hello", () => new { Message = "Hello World" }); 
// �Ǧ^ IResult
app.MapGet("/hello", () => Results.Ok(new { Message = "Hello World" }));
// ���w StatusCode
app.MapGet("/api/todoitems/{id}", async (int id, TodoDb db) =>
         await db.Todos.FindAsync(id) 
         is Todo todo
         ? Results.Ok(todo) 
         : Results.NotFound())
   .Produces<Todo>(StatusCodes.Status200OK) // �Y�� Todo ���O�� 200
   .Produces(StatusCodes.Status404NotFound);
// �Ǧ^ JSON
app.MapGet("/hello", () => Results.Json(new { Message = "Hello World" }));
// �ۭq Status Code
app.MapGet("/405", () => Results.StatusCode(405));
// �ǯ¤�r
app.MapGet("/text", () => Results.Text("This is some text"));
// �Ǧ^ Stream
app.MapGet("/pokemon", async () => 
{
    var stream = await proxyClient.GetStreamAsync("http://consoto/pokedex.json");
    // Proxy the response as JSON
    return Results.Stream(stream, "application/json");
});
// ���s�ɦV
app.MapGet("/old-path", () => Results.Redirect("/new-path"));
// �Ǧ^�ɮ�
app.MapGet("/download", () => Results.File("myfile.text"));

 */
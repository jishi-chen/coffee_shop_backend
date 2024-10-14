using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class FileController : BaseController
    {
        private readonly HttpContext? _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public FileController(IUserService userService, IHttpContextAccessor accessor, IAddressService addressService, ITenantService tenantService) : base(accessor)
        {
            _context = accessor.HttpContext;
        }
        public IActionResult Index()
        {
            var files = Directory.GetFiles(_uploadPath);
            return View(files);
        }

        [HttpGet]
        public IActionResult Download(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return BadRequest("Filename is not provided.");

            var filePath = Path.Combine(_uploadPath, fileName);

            if (!System.IO.File.Exists(filePath)) return NotFound("File does not exist.");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}

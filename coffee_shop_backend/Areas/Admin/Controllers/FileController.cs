using coffee_shop_backend.Controllers;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace coffee_shop_backend.Areas.Admin.Controllers
{
    [Area("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class FileController : BaseController
    {
        private readonly HttpContext? _context;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        public FileController(IDocumentService documentService, IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }
        [HttpGet]
        [Route("Login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (!Directory.Exists(_uploadPath))
                {
                    Directory.CreateDirectory(_uploadPath);
                }

                var filePath = Path.Combine(_uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Please select a file.";
            }

            return View();
        }
    }
}

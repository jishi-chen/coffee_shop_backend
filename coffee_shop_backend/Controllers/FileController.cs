using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;


namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    public class FileController : BaseController
    {
        private readonly HttpContext? _context;
        private readonly IFileService _fileService;

        public FileController(IFileService fileService, IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return View(_fileService.GetAll(null));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            ViewBag.SearchString = searchString;
            return View(_fileService.GetAll(searchString));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Download")]
        public IActionResult Download(int fileStorageId, string fileName)
        {
            try
            {
                var fileBytes = _fileService.DownloadFile(fileStorageId);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

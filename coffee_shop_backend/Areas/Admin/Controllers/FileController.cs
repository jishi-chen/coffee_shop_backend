using coffee_shop_backend.Controllers;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CoffeeShop.Model.ViewModels;

namespace coffee_shop_backend.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Policy = "AdminPolicy")]
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

        [HttpGet]
        [Route("Upload")]
        public IActionResult Upload()
        {
            return View(new FileUploadViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Upload")]
        public async Task<IActionResult> Upload(FileUploadViewModel model)
        {
            if (await _fileService.UploadFileAsync(model))
            {
                SetAlertMsg("上傳成功");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "File upload failed");
            SetAlertMsg("上傳失敗");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public IActionResult Delete(int fileStorageId)
        {
            if (_fileService.DeleteFile(fileStorageId))
            {
                SetAlertMsg("刪除完成");
            }
            else
            {
                SetAlertMsg("刪除失敗");
            }
            return RedirectToAction("Index");
        }
    }
}

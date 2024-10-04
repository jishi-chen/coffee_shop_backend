using CoffeeShop.Model.ViewModels;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class TenantController : BaseController
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService, IHttpContextAccessor accessor) : base(accessor)
        {
            _tenantService = tenantService;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var model = _tenantService.GetAll();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
                return View(_tenantService.GetAll(searchString));
            else
                return View(_tenantService.GetAll());
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            return View(_tenantService.GetFormViewModel(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public IActionResult Edit(TenantFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tenantService.UpdateTenant(model);
                SetAlertMsg("更新完成");
                return RedirectToAction("Index");
            }
            SetAlertMsg("更新失敗");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Delete")]
        public IActionResult Delete(int? id)
        {
            if (_tenantService.DeleteTenant(id))
                SetAlertMsg("刪除完成");
            else
                SetAlertMsg("刪除失敗");
            return RedirectToAction("Index");
        }
    }
}

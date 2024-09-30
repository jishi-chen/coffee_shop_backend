using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using MathNet.Numerics.RootFinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace coffee_shop_backend.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [Authorize(Policy = "AdminPolicy")]
    public class TenantController : BaseController
    {
        private HttpContext? _context;

        public TenantController(IHttpContextAccessor accessor) : base(accessor)
        {
            _context = accessor.HttpContext;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            var model = _db.Tenants.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Index")]
        public IActionResult Index(string searchString)
        {
            var model = new List<Tenant>();
            if (!string.IsNullOrEmpty(searchString))
                model = _db.Tenants.Where(x => x.TenantName.Contains(searchString) || x.ContactName.Contains(searchString)).ToList();
            else
                model = _db.Tenants.ToList();
            return View(model);
        }

        [HttpGet]
        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            var model = new TenantFormViewModel();
            if (id.HasValue)
            {
                var currentTenant = _db.Tenants.Find(id.Value);
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Tenant, TenantFormViewModel>());
                var mapper = config.CreateMapper();
                mapper.Map(currentTenant, model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public IActionResult Edit(TenantFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<TenantFormViewModel, Tenant>());
                var mapper = config.CreateMapper();
                var tenant = new Tenant();
                if (!model.TenantId.HasValue)
                {
                    tenant = mapper.Map<Tenant>(model);
                    tenant.CreateDate = DateTime.Now;
                    tenant.Creator = GetCurrentLoginId();
                    _db.Tenants.Add(tenant);
                }
                else
                {
                    tenant = _db.Tenants.Find(model.TenantId.Value);
                    if (tenant != null)
                    {
                        mapper.Map(model, tenant);
                        tenant.UpdateDate = DateTime.Now;
                        tenant.Updator = GetCurrentLoginId();
                    }
                }
                _db.SaveChanges();
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
            if (id.HasValue)
            {
                var currentTenant = _db.Tenants.Find(id.Value);
                bool isUsed = _db.Users.Where(x => x.TenantId == id.Value).Any();
                if (!isUsed && currentTenant != null)
                {
                    _db.Tenants.Remove(currentTenant);
                    _db.SaveChanges();
                    SetAlertMsg("刪除完成");
                }
            }
            SetAlertMsg("刪除失敗");
            return RedirectToAction("Index");
        }
    }
}

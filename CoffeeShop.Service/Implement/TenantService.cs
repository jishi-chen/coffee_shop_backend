using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace CoffeeShop.Service.Implement
{
    public class TenantService : ITenantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;


        public TenantService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public IEnumerable<Tenant> GetAll(string? searchString)
        {
            var result = _unitOfWork.TenantRepository.GetAll();
            if (searchString != null)
            {
                result = result.Where(x => x.TenantName.Contains(searchString) || x.ContactName.Contains(searchString)).ToList();
            }
            return result;
        }

        public TenantFormViewModel GetFormViewModel(int? id)
        {
            var model = new TenantFormViewModel();
            if (id.HasValue)
            {
                var currentTenant = _unitOfWork.TenantRepository.GetById(id.Value);
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Tenant, TenantFormViewModel>());
                var mapper = config.CreateMapper();
                mapper.Map(currentTenant, model);
            }
            _unitOfWork.Dispose();
            return model;
        }

        public void UpdateTenant(TenantFormViewModel model)
        {
            var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<TenantFormViewModel, Tenant>());
            var mapper = config.CreateMapper();
            var tenant = new Tenant();
            if (!model.TenantId.HasValue)
            {
                tenant = mapper.Map<Tenant>(model);
                tenant.CreateDate = DateTime.Now;
                tenant.Creator = _userService.GetCurrentLoginId();
                _unitOfWork.TenantRepository.Add(tenant);
            }
            else
            {
                tenant = _unitOfWork.TenantRepository.GetById(model.TenantId.Value);
                if (tenant != null)
                {
                    mapper.Map(model, tenant);
                    tenant.UpdateDate = DateTime.Now;
                    tenant.Updator = _userService.GetCurrentLoginId();
                    _unitOfWork.TenantRepository.Update(tenant);
                }
            }
            _unitOfWork.Complete();
        }
        public bool DeleteTenant(int? id)
        {
            if (id.HasValue)
            {
                var currentTenant = _unitOfWork.TenantRepository.GetById(id.Value);
                bool isUsed = _unitOfWork.UserRepository.GetByTenantId(id.Value).Any();
                if (!isUsed && currentTenant != null)
                {
                    _unitOfWork.TenantRepository.Delete(id.Value);
                    _unitOfWork.Complete();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<SelectListItem> GetTenantList(int? tenantId)
        {
            List<SelectListItem> tenantList = new List<SelectListItem>();
            IEnumerable<Tenant> list = _unitOfWork.TenantRepository.GetAll();
            foreach (var c in list)
            {
                if (c.IsEnabled == false) continue;
                tenantList.Add(new SelectListItem()
                {
                    Text = c.TenantName,
                    Value = c.TenantId.ToString(),
                    Selected = tenantId.HasValue ? tenantId.Value == c.TenantId : false,
                });
            }
            return tenantList;
        }
    }
}

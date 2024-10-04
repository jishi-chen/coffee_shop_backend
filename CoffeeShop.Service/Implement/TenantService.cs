using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


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

        public IEnumerable<Tenant> GetAll()
        {
            return _unitOfWork.TenantRepository.GetAll();
        }
        public IEnumerable<Tenant> GetAll(string searchString)
        {
            return _unitOfWork.TenantRepository.GetAll(searchString);
        }

        public Tenant GetById(int id)
        {
            return _unitOfWork.TenantRepository.GetById(id);
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
                    return true;
                }
            }
            return false;
        }
    }
}

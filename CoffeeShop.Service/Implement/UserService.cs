using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Implement;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using CoffeeShop.Utility.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IAddressService _addressService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IAddressService addressService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = new HttpContextAccessor();
            _addressService = addressService;
        }

        public UserRegisterViewModel GetFormViewModel(int? id,ref string cityId, ref string areaId)
        {
            var model = new UserRegisterViewModel();
            if (id.HasValue)
            {
                var user = _unitOfWork.UserRepository.GetById(id.Value);
                var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<CoffeeShop.Model.Entities.User, UserRegisterViewModel>().ForMember(dest => dest.Address, opt => opt.Ignore()));
                var mapper = config.CreateMapper();
                mapper.Map(user, model);
                model.Password = "";
                model.ConfirmPassword = "";
                if (user.AddressId.HasValue)
                {
                    var area = _addressService.GetAddressAreaById(user.AddressId.Value);
                    cityId = area.CityId.ToString();
                    areaId = area.Id.ToString();
                    model.Address.PostalCode = area.ZipCode;
                    model.Address.AddressField = user.Address;
                }
            }
            return model;
        }
        public void UpdateUser(UserRegisterViewModel model)
        {
            //密碼加密
            var passwordHasher = new PasswordHasher<UserRegisterViewModel>();
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<UserRegisterViewModel, User>());
            var mapper = config.CreateMapper();

            var user = new User();
            if (!model.UserId.HasValue)
            {
                user = mapper.Map<User>(model);
                user.PasswordHash = passwordHasher.HashPassword(model, model.Password);
                user.AddressId = int.TryParse(model.Address.Region, out int addressId) ? addressId : -1;
                user.Address = model.Address.AddressField;
                user.CreateDate = DateTime.Now;
                user.Creator = GetCurrentLoginId();
                _unitOfWork.UserRepository.Add(user);
                
            }
            else
            {
                user = _unitOfWork.UserRepository.GetById(model.UserId.Value);
                if (user != null)
                {
                    mapper.Map(model, user);
                    user.AddressId = int.TryParse(model.Address.Region, out int addressId) ? addressId : -1;
                    user.Address = model.Address.AddressField;
                    user.UpdateDate = DateTime.Now;
                    user.Updator = GetCurrentLoginId();
                    _unitOfWork.UserRepository.Update(user);
                }
            }
        }

        public User? CheckPassword(UserLoginViewModel model)
        {
            User? user = _unitOfWork.UserRepository.GetByUserName(model.UserName);
            var passwordHasher = new PasswordHasher<UserLoginViewModel>();
            if (user != null && passwordHasher.VerifyHashedPassword(model, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
            {
                return user;
            }
            return null;
        }

        public int GetCurrentLoginId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                return int.TryParse(userIdClaim?.Value, out int userId) ? userId : 0;
            }
            return 0;
        }
    }
}

using AutoMapper;
using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Ganss.Xss;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace CoffeeShop.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IAddressService _addressService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HtmlSanitizer htmlSanitizer = new HtmlSanitizer();

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IAddressService addressService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = new HttpContextAccessor();
            _addressService = addressService;
        }

        public UserRegisterViewModel GetFormViewModel(int? id, ref string cityId, ref string areaId)
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
                if (user != null && user.AddressId.HasValue)
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
            User? user = _unitOfWork.UserRepository.GetByUserName(htmlSanitizer.Sanitize(model.UserName));
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
        public IEnumerable<UserIndexViewModel> GetIndexViewModel(string? searchString)
        {
            IEnumerable<User> users = _unitOfWork.UserRepository.GetAll();
            IEnumerable<Tenant> tenants = _unitOfWork.TenantRepository.GetAll();
            var result = users.Join(tenants,
                user => user.TenantId,
                tenant => tenant.TenantId,
                (user, tenant) => new UserIndexViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    TenantName = tenant.TenantName,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled
                });
            if (searchString != null)
            {
                result = result.Where(x => x.UserName.Contains(searchString) || x.TenantName.Contains(searchString)).ToList();
            }
            return result;
        }
    }
}

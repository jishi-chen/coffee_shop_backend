using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using CoffeeShop.Service.Interface;
using Microsoft.AspNetCore.Http;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = new HttpContextAccessor();
        }

        public bool CheckPassword()
        {

            return true;
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

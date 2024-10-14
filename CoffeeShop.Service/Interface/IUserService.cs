using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;

namespace CoffeeShop.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserIndexViewModel> GetIndexViewModel(string? searchString);
        UserRegisterViewModel GetFormViewModel(int? id, ref string cityId, ref string areaId);
        void UpdateUser(UserRegisterViewModel model);
        User? CheckPassword(UserLoginViewModel model);
        int GetCurrentLoginId();
    }
}

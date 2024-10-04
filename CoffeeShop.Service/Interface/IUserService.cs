

namespace CoffeeShop.Service.Interface
{
    public interface IUserService
    {
        bool CheckPassword();
        int GetCurrentLoginId();
    }
}

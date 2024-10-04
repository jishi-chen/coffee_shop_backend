using CoffeeShop.Model.Entities;


namespace CoffeeShop.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IEnumerable<User> GetAll(string searchString);
        User? GetByUserName(string name);
        IEnumerable<User> GetByTenantId(int id);
    }
}

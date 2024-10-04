using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using System.Data;

namespace CoffeeShop.Repository.Implement
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbTransaction transaction, CoffeeShopContext context) : base(transaction, context) { }

        public IEnumerable<User> GetAll(string searchString)
        {
            return _context.Users.Where(x => x.UserName.Contains(searchString));
        }
        public User? GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == userName);
        }
        public IEnumerable<User> GetByTenantId(int id)
        {
            return _context.Users.Where(x => x.TenantId == id);
        }
    }
}

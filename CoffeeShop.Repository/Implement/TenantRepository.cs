using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using System.Data;

namespace CoffeeShop.Repository.Implement
{
    public class TenantRepository : GenericRepository<Tenant>, ITenantRepository
    {

        public TenantRepository(IDbTransaction transaction, CoffeeShopContext context): base(transaction, context) { }


    }
}

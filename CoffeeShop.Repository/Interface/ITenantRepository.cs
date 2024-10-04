using CoffeeShop.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Repository.Interface
{
    public interface ITenantRepository : IGenericRepository<Tenant>
    {
        IEnumerable<Tenant> GetAll(string searchString);

    }
}

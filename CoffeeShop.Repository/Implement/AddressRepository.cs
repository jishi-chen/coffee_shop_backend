using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShop.Repository.Implement
{
    public class AddressRepository : GenericRepository<AddressArea>, IAddressRepository
    {
        public AddressRepository(IDbTransaction transaction, CoffeeShopContext context) : base(transaction, context) { }

        public IEnumerable<AddressCity> GetAddressCity()
        {
            return _context.AddressCities;
        }
        public IEnumerable<AddressArea> GetAddressAreaByCityId(int cityId)
        {
            return _context.AddressAreas.Where(x => x.CityId == cityId);
        }
    }
}

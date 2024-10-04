using CoffeeShop.Model.Entities;

namespace CoffeeShop.Repository.Interface
{
    public interface IAddressRepository : IGenericRepository<AddressArea>
    {
        IEnumerable<AddressCity> GetAddressCity();
        IEnumerable<AddressArea> GetAddressAreaByCityId(int cityId);

    }
}

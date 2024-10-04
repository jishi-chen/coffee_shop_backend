using CoffeeShop.Repository.Interface;

namespace CoffeeShop.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository DocumentRepository { get; }
        ITenantRepository TenantRepository { get; }
        IUserRepository UserRepository { get; }
        IAddressRepository AddressRepository { get; }

        void Complete();
        void Rollback();
    }
}

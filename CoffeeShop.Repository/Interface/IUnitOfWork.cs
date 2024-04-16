using CoffeeShop.Repository.Interface;

namespace CoffeeShop.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository DocumentRepository { get; }

        void Complete();
        void Rollback();
    }
}

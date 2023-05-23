namespace coffee_shop_backend.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentRepository DocumentRepository { get; }

        void Complete();
        void Rollback();
    }
}

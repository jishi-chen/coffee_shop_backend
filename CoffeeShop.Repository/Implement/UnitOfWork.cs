using CoffeeShop.Model;
using CoffeeShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoffeeShop.Repository.Implement
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeShopContext _context;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IDocumentRepository _documentRepository;

        public UnitOfWork(CoffeeShopContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IDocumentRepository DocumentRepository
        {
            get { return _documentRepository ?? (_documentRepository = new DocumentRepository(_transaction)); }
        }

        public void Complete()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
        }


        public void Dispose()
        {
            _transaction?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

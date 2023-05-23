using coffee_shop_backend.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace coffee_shop_backend.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed = false;
        private IDocumentRepository _documentRepository;
        private IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

        public UnitOfWork(string conStr = "")
        {
            _connection = new SqlConnection();
            if (_connection.State == ConnectionState.Closed)
            {
                CreateConnection(conStr);
            }
        }

        /// <summary> 建立資料庫連接 </summary>
        /// <param name="conStr">SQL連接字串</param>
        private void CreateConnection(string conStr)
        {
            conStr = configuration.GetConnectionString("DefaultConnection");
            _connection = new SqlConnection(conStr);
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
                ResetRepositories();
            }
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = _connection.BeginTransaction();
            ResetRepositories();
        }

        private void ResetRepositories()
        {
            _documentRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}

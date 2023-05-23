using coffee_shop_backend.Interface;
using System.Data;

namespace coffee_shop_backend.Repository
{
    public class GenericRepository : IGenericRepository
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection => Transaction.Connection;

        public GenericRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
    }
}

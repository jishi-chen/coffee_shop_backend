using CoffeeShop.Repository.Interface;
using System.Data;

namespace CoffeeShop.Repository.Implement
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

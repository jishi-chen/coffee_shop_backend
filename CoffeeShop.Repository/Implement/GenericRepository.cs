using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CoffeeShop.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection? Connection => Transaction.Connection;

        protected readonly CoffeeShopContext _context;

        public GenericRepository(IDbTransaction transaction, CoffeeShopContext context)
        {
            Transaction = transaction;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            T? entity = GetById(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                _context.SaveChanges();
            }
        }

        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
    }
}

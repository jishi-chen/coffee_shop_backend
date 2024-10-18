using CoffeeShop.Model;
using CoffeeShop.Model.Entities;
using CoffeeShop.Repository.Interface;
using System.Data;


namespace CoffeeShop.Repository.Implement
{
    public class FileRepository : GenericRepository<FileStorage>, IFileRepository
    {
        public FileRepository(IDbTransaction transaction, CoffeeShopContext context) : base(transaction, context) { }

    }
}

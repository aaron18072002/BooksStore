using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.Repositories
{
    public class ProductCategory : Repository<Product>, IProductRepository
    {
        public ProductCategory(BooksStoreDbContext db) : base(db) 
        {
        }

        public async Task Update(Product product)
        {
            base.DbSet.Update(product);
            await base._db.SaveChangesAsync();
        }
    }
}

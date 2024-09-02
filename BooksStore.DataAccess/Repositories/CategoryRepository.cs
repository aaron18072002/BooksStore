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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BooksStoreDbContext db) : base(db)
        {

        }
        public Task Save()
        {
            throw new NotImplementedException();
        }

        public Task Update(Category category)
        {
            throw new NotImplementedException();
        }
    }
}

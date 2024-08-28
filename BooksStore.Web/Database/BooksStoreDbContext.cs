using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Database
{
    public class BooksStoreDbContext : DbContext
    {
        public BooksStoreDbContext(DbContextOptions<BooksStoreDbContext> options) : base(options)
        {

        }
    }
}

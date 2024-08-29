using BooksStoreRazor.Temp.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksStoreRazor.Temp.Database
{
    public class BooksStoreRazorDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public BooksStoreRazorDbContext(DbContextOptions<BooksStoreRazorDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

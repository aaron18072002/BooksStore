using BooksStore.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Database
{
    public class BooksStoreDbContext : DbContext
    {
        public BooksStoreDbContext(DbContextOptions<BooksStoreDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seed data for category table
            var categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Action",
                    DisplayOrder = 1
                },
                new Category()
                {
                    Id = 2,
                    Name = "Scifi",
                    DisplayOrder = 2
                },
                new Category()
                {
                    Id = 3,
                    Name = "History",
                    DisplayOrder = 3
                },
            };
            foreach (var category in categories)
            {
                modelBuilder.Entity<Category>().HasData(category);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

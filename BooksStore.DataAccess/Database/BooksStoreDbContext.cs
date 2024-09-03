using BooksStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace BooksStore.DataAccess.Database
{
    public class BooksStoreDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public BooksStoreDbContext(DbContextOptions<BooksStoreDbContext> options) : base(options)
        {

        }

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

            //seed data for products table
            var stringBuilder = new StringBuilder();
            using (var streamReader = new StreamReader(@"D:\\aspnetcore\\BooksStoreSolution\\BooksStore.Web\\products.json"))
            {
                string? line;
                while((line = streamReader.ReadLine()) != null)
                {
                    stringBuilder.AppendLine(line);
                }
            };
            var products = JsonSerializer.Deserialize<List<Product>>(stringBuilder.ToString());
            if(products != null)
            {
                foreach (var product in products)
                {
                    modelBuilder.Entity<Product>().HasData(product);
                }
            }

            //Add foreign key
            modelBuilder.Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(c => c.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}

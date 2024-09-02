using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.Models.DTOs
{
    public class ProductResponse
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? ISBN { get; set; }

        public string? Author { get; set; }

        public decimal ListPrice { get; set; }

        public decimal Price { get; set; }

        public decimal Price50 { get; set; }

        public decimal Price100 { get; set; }
    }
    public static class ProductExtensions
    {
        public static ProductResponse ToProductResponse(this Product product)
        {
            return new ProductResponse()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ISBN = product.ISBN,
                Author = product.Author,
                ListPrice = product.ListPrice,
                Price = product.Price,
                Price50 = product.Price50,
                Price100 = product.Price100             
            };
        }
        public static ProductUpdateRequest ToProductUpdateRequest(this Product product)
        {
            return new ProductUpdateRequest()
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                ISBN = product.ISBN,
                Author = product.Author,
                ListPrice = product.ListPrice,
                Price = product.Price,
                Price50 = product.Price50,
                Price100 = product.Price100
            };
        }
    }
}

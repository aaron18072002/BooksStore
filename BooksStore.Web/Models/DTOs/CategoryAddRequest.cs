using System.ComponentModel.DataAnnotations;

namespace BooksStore.Web.Models.DTOs
{
    public class CategoryAddRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(40, ErrorMessage = "Category name must be less than 40 characters")]
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
        public Category ToCategory()
        {
            return new Category()
            {
                Name = this.Name,
                DisplayOrder = this.DisplayOrder
            };
        }
    }
}

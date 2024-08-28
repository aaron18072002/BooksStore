using System.ComponentModel.DataAnnotations;

namespace BooksStore.Web.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(40, ErrorMessage = "Category name is too long")]
        public string? Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}

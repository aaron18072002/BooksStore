using System.ComponentModel.DataAnnotations;

namespace BooksStore.Web.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required()]
        [MaxLength(40)]
        public string? Name { get; set; }

        [Range(1, 100)]
        public int DisplayOrder { get; set; }
    }
}

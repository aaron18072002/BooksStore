using BooksStoreRazor.Temp.Database;
using BooksStoreRazor.Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BooksStoreRazor.Temp.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly BooksStoreRazorDbContext _db;
        public Category Category { get; set; }
        public CreateModel(BooksStoreRazorDbContext db)
        {
            this._db = db;
            this.Category = new Category();
        }
        public void OnGet()
        {
        }
    }
}

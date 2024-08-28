using BooksStore.Web.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Controllers
{
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly BooksStoreDbContext _db;
        public CategoryController(BooksStoreDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            var categories = await this._db.Categories.ToListAsync();

            return View(categories);
        }
    }
}

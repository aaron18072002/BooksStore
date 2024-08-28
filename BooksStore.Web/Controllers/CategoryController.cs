using BooksStore.Web.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Controllers
{
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly BooksStoreDbContext _db;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(BooksStoreDbContext db, ILogger<CategoryController> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action method",
                nameof(CategoryController), nameof(this.Index));
            var categories = await this._db.Categories.ToListAsync();

            return View(categories);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action method",
                nameof(CategoryController), nameof(this.Create));

            return View();
        }
    }
}

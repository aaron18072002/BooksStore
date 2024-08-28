using BooksStore.Web.Database;
using BooksStore.Web.Models.DTOs;
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

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm]CategoryAddRequest categoryAddRequest)
        {
            if(categoryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryAddRequest));
            }

            var category = categoryAddRequest.ToCategory();
            //category.Id = categoryAddRequest.DisplayOrder;

            this._db.Categories.Add(category);
            await this._db.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }
    }
}

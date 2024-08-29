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
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Index));
            var categories = await this._db.Categories.ToListAsync();

            var categoriesResponse = categories.Select(c => c.ToCategoryResponse());

            return View(categoriesResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Create));

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm]CategoryAddRequest categoryAddRequest)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action post method",
                nameof(CategoryController), nameof(this.Create));

            if (categoryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(categoryAddRequest));
            }

            if(categoryAddRequest.Name == null)
            {
                throw new ArgumentException(nameof(categoryAddRequest));
            }

            if(categoryAddRequest.Name.ToLower() == categoryAddRequest.DisplayOrder.ToString())
            {
                this.ModelState.AddModelError
                    (nameof(categoryAddRequest.Name), "Category name and display order can not be same");
            }

            if(this.ModelState.IsValid)
            {
                var category = categoryAddRequest.ToCategory();

                this._db.Categories.Add(category);
                await this._db.SaveChangesAsync();

                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromQuery]int? categoryId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Edit));

            if(categoryId == null)
            {
                return NotFound();
            }

            var category = await this._db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null) 
            {
                return NotFound();
            }

            var categoryUpdateRequest = category.ToCategoryUpdateRequest();

            return View(categoryUpdateRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm]CategoryUpdateRequest categoryUpdateRequest)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CategoryController), nameof(this.Edit));

            if(!this.ModelState.IsValid)
            {
                return View(categoryUpdateRequest);
            }

            var category = categoryUpdateRequest.ToCategory();
            this._db.Categories.Update(category);
            await this._db.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }
    }
}

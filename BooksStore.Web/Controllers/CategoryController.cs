using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Controllers
{
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryRepository categoryRepo, ILogger<CategoryController> logger)
        {
            this._categoryRepo = categoryRepo;
            this._logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Index));
            var categories = await this._categoryRepo.GetAll();

            var categoriesResponse = categories.Select(c => c.ToCategoryResponse()).ToList();

            return this.View(categoriesResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Create));

            return this.View();
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

                await this._categoryRepo.Update(category);

                this.TempData["Success"] = "Create category succesfully";

                return this.RedirectToAction("Index", "Category");
            }

            return this.View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromQuery]int? categoryId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Edit));

            if(categoryId == null)
            {
                return this.NotFound();
            }

            var category = await this._categoryRepo.GetDetails(c => c.Id == categoryId);
            if (category == null) 
            {
                return this.NotFound();
            }

            var categoryUpdateRequest = category.ToCategoryUpdateRequest();

            return this.View(categoryUpdateRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm]CategoryUpdateRequest categoryUpdateRequest)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CategoryController), nameof(this.Edit));

            if(!this.ModelState.IsValid)
            {
                return this.View(categoryUpdateRequest);
            }

            var category = categoryUpdateRequest.ToCategory();
            await this._categoryRepo.Update(category);

            this.TempData["Success"] = "Update category successfully";

            return this.RedirectToAction("Index", "Category");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromQuery]int? categoryId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Delete));

            if (categoryId == null)
            {
                return this.NotFound();
            };

            var category = await this._categoryRepo.GetDetails(c => c.Id == categoryId);
            if(category == null)
            {
                return this.NotFound();
            }

            var categoryResponse = category.ToCategoryResponse();

            return this.View(categoryResponse);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeletePOST([FromForm]int? Id)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} delete action method",
                nameof(CategoryController), nameof(this.DeletePOST));

            if (Id == null)
            {
                return this.NotFound();
            }

            var category = await this._categoryRepo.GetDetails(c => c.Id == Id);
            if (category == null)
            {
                return this.NotFound();
            }
            
            await this._categoryRepo.Remove(category);

            this.TempData["Success"] = "Delete category successfully";

            return this.RedirectToAction("Index", "Category");
        }
    }
}

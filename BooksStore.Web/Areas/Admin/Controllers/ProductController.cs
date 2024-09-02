using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Index));
            var products = await _unitOfWork.Products.GetAll();

            var categoriesResponse = products.Select(p => p.ToProductResponse()).ToList();

            return View(categoriesResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Create));

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm] ProductAddRequest productAddRequest)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action post method",
                nameof(CategoryController), nameof(this.Create));

            if (productAddRequest == null)
            {
                throw new ArgumentNullException(nameof(productAddRequest));
            }

            if (productAddRequest.Title == null)
            {
                throw new ArgumentException(nameof(productAddRequest));
            }

            if (ModelState.IsValid)
            {
                var product = productAddRequest.ToProduct();

                await _unitOfWork.Products.Add(product);
                await _unitOfWork.Save();

                TempData["Success"] = "Create product succesfully";

                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromQuery] int? productId)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Edit));

            if (productId == null)
            {
                return NotFound();
            }

            var product = await this._unitOfWork.Products.GetDetails(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var productUpdateRequest = product.ToProductUpdateRequest();

            return View(productUpdateRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateRequest productUpdateRequest)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CategoryController), nameof(this.Edit));

            if (!ModelState.IsValid)
            {
                return View(productUpdateRequest);
            }

            var product = productUpdateRequest.ToProduct();

            await this._unitOfWork.Products.Update(product);
            await this._unitOfWork.Save();

            TempData["Success"] = "Update product successfully";

            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Delete([FromQuery] int? productId)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} get action method",
                nameof(CategoryController), nameof(this.Delete));

            if (productId == null)
            {
                return NotFound();
            };

            var product = await this._unitOfWork.Products.GetDetails(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }

            var productResponse = product.ToProductResponse();

            return View(productResponse);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> DeletePOST([FromForm] int? Id)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} delete action method",
                nameof(CategoryController), nameof(this.DeletePOST));

            if (Id == null)
            {
                return NotFound();
            }

            var product = await this._unitOfWork.Products.GetDetails(p => p.Id == Id);
            if (product == null)
            {
                return NotFound();
            }

            await this._unitOfWork.Products.Remove(product);
            await this._unitOfWork.Save();

            TempData["Success"] = "Delete product successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}

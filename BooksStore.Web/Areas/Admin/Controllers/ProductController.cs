using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using BooksStore.Models.DTOs;
using BooksStore.Models.ViewModels;
using BooksStore.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[area]/[controller]")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger, IWebHostEnvironment webHostEnvironment)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
            this._webHostEnvironment = webHostEnvironment;
        }

        private ProductResponse ConvertProductToProductResponse(Product product)
        {
            var productResponse = product.ToProductResponse();
            productResponse.CategoryName = product?.Category?.Name;

            return productResponse;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Index));
            var products = await _unitOfWork.Products.GetAll(includeProperties: "Category");

            var productsResponse = products.Select
                (p => this.ConvertProductToProductResponse(p)).ToList();

            return View(productsResponse);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CategoryController), nameof(this.Create));

            var categories = await _unitOfWork.Categories.GetAll();

            var categoriesList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            this.ViewBag.Categories = categoriesList;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create
            ([FromForm] ProductCreateVM productCreateVM)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} action post method",
                nameof(CategoryController), nameof(this.Create));

            if (productCreateVM == null)
            {
                throw new ArgumentNullException(nameof(productCreateVM));
            }

            if (productCreateVM.ProductAddRequest == null)
            {
                throw new ArgumentException(nameof(productCreateVM));
            }

            var wwwRootPath = this._webHostEnvironment.WebRootPath;
            if (productCreateVM.File != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension
                    (productCreateVM.File.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    productCreateVM.File.CopyTo(fileStream);
                }

                productCreateVM.ProductAddRequest.ImageUrl = @"\images\product\" + fileName;

                var product = productCreateVM.ProductAddRequest.ToProduct();

                await this._unitOfWork.Products.Add(product);
                await this._unitOfWork.Save();

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

            var categories = await _unitOfWork.Categories.GetAll();

            var categoriesList = categories.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            this.ViewBag.Categories = categoriesList;

            var productUpdateVM = new ProductUpdateVM()
            {
                ProductUpdateRequest = productUpdateRequest,
            };

            return View(productUpdateVM);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Edit([FromForm] ProductUpdateVM productUpdateVM)
        {
            _logger.LogInformation("{ControllerName}.{MethodName} post action method",
                nameof(CategoryController), nameof(this.Edit));

            var wwwRootPath = this._webHostEnvironment.WebRootPath;        

            if (productUpdateVM.File != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension
                           (productUpdateVM.File.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    productUpdateVM.File.CopyTo(fileStream);
                }

                if (productUpdateVM.ProductUpdateRequest.ImageUrl != null)
                {
                    var oldImagePath =
                               Path.Combine(wwwRootPath, productUpdateVM.ProductUpdateRequest.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                productUpdateVM.ProductUpdateRequest.ImageUrl = @"\images\product\" + fileName;

            }

            var product = productUpdateVM.ProductUpdateRequest.ToProduct();

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

            var product = await this._unitOfWork.Products.GetDetails
                (p => p.Id == productId, includeProperties: "Category");
            if (product == null)
            {
                return NotFound();
            }

            var productResponse = this.ConvertProductToProductResponse(product);

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

        #region APIs
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllProducts()
        {
            var productsList = await this._unitOfWork.Products.GetAll(includeProperties: "Category");
            var productsResponseList = productsList.Select
                (p => this.ConvertProductToProductResponse(p)).ToList();

            return Json(new
            {
                Data = productsResponseList
            });
        }
        #endregion
    }
}

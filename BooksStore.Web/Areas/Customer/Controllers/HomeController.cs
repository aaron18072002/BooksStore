using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
using BooksStore.Models.DTOs;
using BooksStore.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BooksStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }

        private ProductResponse ConvertProductToProductResponse(Product product)
        {
            var productResponse = product.ToProductResponse();
            productResponse.CategoryName = product?.Category?.Name;

            return productResponse;
        }

        [HttpGet]
        [Route("/")]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(HomeController), nameof(this.Index));

            var products = await this._unitOfWork.Products.GetAll(includeProperties: "Category"); 

            var productsResponse = products.Select(p => this.ConvertProductToProductResponse(p)).ToList();

            return View(productsResponse);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

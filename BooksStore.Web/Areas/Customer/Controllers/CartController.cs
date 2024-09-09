using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]")]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CartController(ILogger<CartController> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CartController), nameof(this.Index));

            var claimsIdentity = (ClaimsIdentity?)this.User.Identity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var shoppingCartsList = await this._unitOfWork.ShoppingCarts.GetAll
                (s => s.ApplicationUserId == userId, includeProperties: "Product");

            var shoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCarts = shoppingCartsList,
                
            };

            return View(shoppingCartVM);
        }
    }
}

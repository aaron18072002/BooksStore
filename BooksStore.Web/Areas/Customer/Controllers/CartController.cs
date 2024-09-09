using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models;
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

        private decimal? GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if(shoppingCart.Count <= 50)
            {
                return shoppingCart?.Product?.Price;
            }
            else
            {
                if(shoppingCart.Count <= 100)
                {
                    return shoppingCart?.Product?.Price50;
                }
                return shoppingCart?.Product?.Price100;
            }
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

            shoppingCartVM.TotalPrice = 0;

            foreach (var shoppingCart in shoppingCartsList)
            {
                var price = this.GetPriceBasedOnQuantity(shoppingCart);
                shoppingCartVM.TotalPrice += price * shoppingCart.Count;
            }

            return View(shoppingCartVM);
        }
    }
}

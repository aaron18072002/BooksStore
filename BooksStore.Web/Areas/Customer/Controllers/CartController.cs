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

            var shoppingCartVM = new ShoppingCartVM();

            shoppingCartVM.TotalPrice = 0;

            foreach (var shoppingCart in shoppingCartsList)
            {
                var price = this.GetPriceBasedOnQuantity(shoppingCart);
                shoppingCart.Price = price;
                shoppingCartVM.TotalPrice += price * shoppingCart.Count;
            }

            shoppingCartVM.ShoppingCarts = shoppingCartsList;

            return View(shoppingCartVM);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Summary()
        {
			this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
				nameof(CartController), nameof(this.Summary));

			return View();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Plus([FromQuery]int? cartId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CartController), nameof(this.Plus));

            if (cartId == null)
            {
                return this.NotFound();
            }
            var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
                (s => s.Id == cartId);
            if (shoppingCart == null)
            {
                return this.NotFound();
            }

            shoppingCart.Count += 1;
            await this._unitOfWork.ShoppingCarts.Update(shoppingCart);
            await this._unitOfWork.Save();

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Minus([FromQuery] int? cartId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CartController), nameof(this.Minus));

            if (cartId == null)
            {
                return this.NotFound();
            }
            var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
                (s => s.Id == cartId);
            if (shoppingCart == null)
            {
                return this.NotFound();
            }

            if(shoppingCart.Count <= 1)
            {
                await this._unitOfWork.ShoppingCarts.Remove(shoppingCart);  
            }
            else
            {
                shoppingCart.Count -= 1;
                await this._unitOfWork.ShoppingCarts.Update(shoppingCart);
                await this._unitOfWork.Save();
            } 

            return this.RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Remove([FromQuery] int? cartId)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CartController), nameof(this.Remove));

            if (cartId == null)
            {
                return this.NotFound();
            }
            var shoppingCart = await this._unitOfWork.ShoppingCarts.GetDetails
                (s => s.Id == cartId);
            if (shoppingCart == null)
            {
                return this.NotFound();
            }

            await this._unitOfWork.ShoppingCarts.Remove(shoppingCart);
            await this._unitOfWork.Save();

            return this.RedirectToAction("Index", "Cart");
        }
    }
}

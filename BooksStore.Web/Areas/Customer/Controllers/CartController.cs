using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("[area]/[controller]")]
    public class CartController : Controller
    {
        private ILogger<CartController> _logger;
        public CartController(ILogger<CartController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(CartController), nameof(this.Index));

            return View();
        }
    }
}

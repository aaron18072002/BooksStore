using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Models.ViewModels;
using BooksStore.Utilities;
using BooksStore.Web.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
    //[Authorize(Roles = $"{StaticDetails.Role_Admin},{StaticDetails.Role_Employee}")]
    [Authorize]
	[Route("[area]/[controller]")]
	public class OrderController : Controller
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IUnitOfWork _unitOfWork;
		public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
		{
			this._logger = logger;
			this._unitOfWork = unitOfWork;
		}

		[HttpGet]
		[Route("[action]")]
		public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> OrderDetails([FromQuery]int? orderId)
        {
            if(!orderId.HasValue)
            {
                return this.NotFound();
            }
            var orderHeader = await this._unitOfWork.OrderHeaders.GetDetails
                (oh => oh.Id == orderId, includeProperties: "ApplicationUser");
            if(orderHeader == null)
            {
                return this.NotFound();
            }
            var listOfOrderDetails = await this._unitOfWork.OrderDetails.GetAll
                (od => od.OrderHeaderId == orderHeader.Id);

            var orderVM = new OrderVM()
            {
                OrderHeader = orderHeader,
                OrderDetails = listOfOrderDetails
            };

            return View(orderVM);
        }

        #region API Calls
        [HttpGet]
		[Route("[action]")]
		public async Task<IActionResult> GetAllOrders([FromQuery]string? status)
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action API get method",
				nameof(OrderController), nameof(this.GetAllOrders));

			var orderHeaders = await this._unitOfWork.OrderHeaders.GetAll
				(includeProperties: "ApplicationUser");

			switch(status)
			{
                case "pending":
                    orderHeaders = orderHeaders.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusPending);
                    break;
                case "delayed":
                    orderHeaders = orderHeaders.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(o => o.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }

            return this.Json(new
            {
                Data = orderHeaders
            });
        }
		#endregion
	}
}

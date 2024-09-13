using BooksStore.DataAccess.Repositories.IRepositories;
using BooksStore.Utilities;
using BooksStore.Web.Areas.Customer.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
	//[Authorize(Roles = $"{StaticDetails.Role_Admin},{StaticDetails.Role_Employee}")]
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

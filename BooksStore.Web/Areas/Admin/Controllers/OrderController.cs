﻿using BooksStore.DataAccess.Repositories.IRepositories;
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
		public async Task<IActionResult> GetAllOrders()
		{
			this._logger.LogInformation("{ControllerName}.{MethodName} action API get method",
				nameof(OrderController), nameof(this.GetAllOrders));

			var orderHeaders = await this._unitOfWork.OrderHeaders.GetAll
				(includeProperties: "ApplicationUser");

            return this.Json(new
            {
                Data = orderHeaders
            });
        }
		#endregion
	}
}

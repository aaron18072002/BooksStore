using BooksStore.DataAccess.Database;
using BooksStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = StaticDetails.Role_Admin)]
    //[Authorize]
    [Route("[area]/[controller]")]
    public class UserController : Controller
    {
        private readonly BooksStoreDbContext _db;
        private readonly ILogger<UserController> _logger;
        public UserController(BooksStoreDbContext db, ILogger<UserController> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Index()
        {
            return this.View();
        }

        #region API calls
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await this._db.ApplicationUsers
                .Include(u => u.Company).ToListAsync();

            return this.Json(new
            {
                Data = users
            });
        }
        #endregion
    }
}

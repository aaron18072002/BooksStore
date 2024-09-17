using BooksStore.DataAccess.Database;
using BooksStore.Models;
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

            var usersRoles = await this._db.UserRoles.ToListAsync();
            var roles = await this._db.Roles.ToListAsync();

            foreach (var user in users)
            {
                var roleId = usersRoles.FirstOrDefault(u => u.UserId == user.Id)?.RoleId;
                if (roleId == null)
                {
                    user.Role = "Didn't assign role yet";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(r => r.Id == roleId)?.Name;
                }

                if (user.Company == null)
                {
                    user.Company = new Company();
                    user.Company.Name = string.Empty;
                }
            }

            return this.Json(new
            {
                Data = users
            });
        }
        #endregion
    }
}

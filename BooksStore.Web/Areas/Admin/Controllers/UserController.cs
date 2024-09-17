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
            this._logger.LogInformation("{ControllerName}.{MethodName} action get method",
                nameof(UserController), nameof(this.Index));

            return this.View();
        }

        #region API calls
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action get API method",
                nameof(UserController), nameof(this.GetAllUsers));

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

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> LockUser([FromQuery]string? id)
        {
            this._logger.LogInformation("{ControllerName}.{MethodName} action POST API method",
                nameof(UserController), nameof(this.LockUser));

            var userFromDb = await this._db.ApplicationUsers.FirstOrDefaultAsync
                (u => u.Id == id);
            if (userFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked and we need to unlock them
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            await this._db.SaveChangesAsync();

            return this.Json(new { success = true, message = "Lock User Successful" });
        }
        #endregion
    }
}

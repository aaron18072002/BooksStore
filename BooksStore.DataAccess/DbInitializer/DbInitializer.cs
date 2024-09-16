using BooksStore.DataAccess.Database;
using BooksStore.Models;
using BooksStore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksStore.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BooksStoreDbContext _db;
        public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, BooksStoreDbContext db)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._db = db;
        }

        public async Task Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (this._db.Database.GetPendingMigrations().Count() > 0)
                {
                    this._db.Database.Migrate();
                }
            }
            catch (Exception ex) { }

            //create roles if they are not created
            if (!this._roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
            {
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                this._roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                await this._userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin1",
                    Email = "admin1@gmail.com",
                    Name = "Nguyễn Thanh Anh",
                    PhoneNumber = "0906413506",
                    StreetAddress = "54 Hải Hồ",
                    State = "Hải Châu",
                    PostalCode = "23422",
                    City = "Đà Nẵng"
                }, "@Nguyenthanhanh123");

                var user = await this._db.ApplicationUsers.FirstOrDefaultAsync
                    (u => u.Email == "admin1@gmail.com");

                if (user != null)
                {                   
                    await this._userManager.AddToRoleAsync(user, StaticDetails.Role_Admin);
                }
            }

            return;
        }
    }
}

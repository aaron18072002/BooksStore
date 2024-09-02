using BooksStore.DataAccess.Database;
using BooksStore.DataAccess.Repositories;
using BooksStore.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BooksStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<BooksStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestHeaders.Add("sec-ch-ua");
                logging.ResponseHeaders.Add("MyResponseHeader");
                logging.MediaTypeOptions.AddText("application/javascript");
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
                logging.CombineLogs = true;
            });

            // Serilog
            builder.Host.UseSerilog((context, services, loggerConfiguration) =>
            {
                loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

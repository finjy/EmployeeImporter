using Microsoft.EntityFrameworkCore;
using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Core.Application.Services;
using EmployeeImporter.Core.Domain.Interfaces;
using EmployeeImporter.Infrastructure.Data.Context;
using EmployeeImporter.Infrastructure.Data.Repositories;
using EmployeeImporter.Infrastructure.Services;
using DataTables.AspNet.AspNetCore;

namespace EmployeeImporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register Services
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<ICsvImportService, CsvImportService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();

            // Register DataTables.AspNet
            builder.Services.RegisterDataTables();

            var app = builder.Build();

            // Apply migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    // Await for the database to be available
                    int retries = 10;
                    while (retries > 0)
                    {
                        try
                        {
                            logger.LogInformation("Attempting to connect to database...");
                            context.Database.OpenConnection();
                            context.Database.CloseConnection();
                            break;
                        }
                        catch (Exception ex)
                        {
                            retries--;
                            logger.LogWarning($"Failed to connect to database. {retries} retries left. Error: {ex.Message}");
                            if (retries == 0)
                            {
                                logger.LogError("Failed to connect to database after multiple attempts.");
                                throw;
                            }
                            Task.Delay(5000).Wait(); // Pause for 5 seconds before retrying
                        }
                    }

                    logger.LogInformation("Connection to database successful. Running migrations...");
                    context.Database.Migrate();
                    logger.LogInformation("Migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

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
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

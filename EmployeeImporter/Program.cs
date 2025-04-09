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

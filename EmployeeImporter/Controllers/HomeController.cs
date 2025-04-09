using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeImporter.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Models;
using DataTables.AspNet.Core;
using DataTables.AspNet.AspNetCore;
using System.Diagnostics;

namespace EmployeeImporter.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        public HomeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "File is not selected or empty";
                return RedirectToAction(nameof(Index));
            }
            if (!file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "File format must be CSV";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                using var stream = file.OpenReadStream();
                var processedCount = await _employeeService.ImportEmployeesFromCsvAsync(stream);
                TempData["Success"] = $"Imported {processedCount} records";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error importing file: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync("surname_asc");
            return Json(employees);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employee);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating record");
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

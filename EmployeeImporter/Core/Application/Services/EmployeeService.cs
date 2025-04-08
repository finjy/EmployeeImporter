using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Core.Domain.Entities;
using EmployeeImporter.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeImporter.Core.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICsvImportService _csvImportService;

        public EmployeeService(IEmployeeRepository employeeRepository, ICsvImportService csvImportService)
        {
            _employeeRepository = employeeRepository;
            _csvImportService = csvImportService;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(string sortOrder)
        {
            var employees = await _employeeRepository.GetAllAsync();
            return SortEmployees(employees, sortOrder);
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm, string sortOrder)
        {
            var employees = await _employeeRepository.SearchAsync(searchTerm);
            return SortEmployees(employees, sortOrder);
        }

        public async Task<int> ImportEmployeesFromCsvAsync(Stream fileStream)
        {
            var result = await _csvImportService.ImportEmployeesFromCsvAsync(fileStream);
            await _employeeRepository.AddRangeAsync(result.employees);
            await _employeeRepository.SaveChangesAsync();
            return result.processedCount;
        }

        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _employeeRepository.Update(employee);
            await _employeeRepository.SaveChangesAsync();
            return employee;
        }

        private IEnumerable<Employee> SortEmployees(IEnumerable<Employee> employees, string sortOrder)
        {
            return sortOrder switch
            {
                "surname_desc" => employees.OrderByDescending(e => e.Surname),
                _ => employees.OrderBy(e => e.Surname), // Default: surname ascending
            };
        }
    }
}

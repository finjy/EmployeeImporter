using EmployeeImporter.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeImporter.Core.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(string sortOrder);
        Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm, string sortOrder);
        Task<int> ImportEmployeesFromCsvAsync(Stream fileStream);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
    }
}

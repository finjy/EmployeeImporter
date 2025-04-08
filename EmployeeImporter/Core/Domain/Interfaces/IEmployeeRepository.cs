using EmployeeImporter.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeImporter.Core.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetBySurnameAsync(string surname);
        Task<IEnumerable<Employee>> SearchAsync(string searchTerm);
        Task<Employee> GetByIdAsync(int id);
        Task AddAsync(Employee employee);
        Task AddRangeAsync(IEnumerable<Employee> employees);
        void Update(Employee employee);
        Task<int> SaveChangesAsync();
    }
}

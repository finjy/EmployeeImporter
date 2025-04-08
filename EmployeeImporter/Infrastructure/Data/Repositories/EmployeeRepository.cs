using EmployeeImporter.Core.Domain.Entities;
using EmployeeImporter.Core.Domain.Interfaces;
using EmployeeImporter.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeImporter.Infrastructure.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetBySurnameAsync(string surname)
        {
            return await _context.Employees
                .Where(e => e.Surname.Contains(surname))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllAsync();

            return await _context.Employees
                .Where(e => e.Surname.Contains(searchTerm) ||
                            e.Forenames.Contains(searchTerm) ||
                            e.EmailHome.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
        }

        public async Task AddRangeAsync(IEnumerable<Employee> employees)
        {
            await _context.Employees.AddRangeAsync(employees);
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

using EmployeeImporter.Core.Domain.Entities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EmployeeImporter.Core.Application.Interfaces
{
    public interface ICsvImportService
    {
        Task<(List<Employee> employees, int processedCount)> ImportEmployeesFromCsvAsync(Stream fileStream);
    }
}

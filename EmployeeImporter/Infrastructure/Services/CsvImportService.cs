using CsvHelper;
using CsvHelper.Configuration;
using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeImporter.Infrastructure.Services
{
    public class CsvImportService : ICsvImportService
    {
        public async Task<(List<Employee> employees, int processedCount)> ImportEmployeesFromCsvAsync(Stream fileStream)
        {
            // Обертка Task.Run для совместимости с async/await
            return await Task.Run(() => {
                // Create the CsvHelper configuration
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    Delimiter = ",",
                    TrimOptions = TrimOptions.Trim
                };

                using var reader = new StreamReader(fileStream);
                using var csv = new CsvReader(reader, configuration);

                // Map CSV columns to Employee properties
                csv.Context.RegisterClassMap<EmployeeCsvMap>();

                // Read the CSV records
                var records = csv.GetRecords<EmployeeCsvRecord>();
                var employees = new List<Employee>();

                foreach (var record in records)
                {
                    // Convert CsvRecord to Employee entity
                    var employee = new Employee
                    {
                        PayrollNumber = record.PayrollNumber,
                        Forenames = record.Forenames,
                        Surname = record.Surname,
                        DateOfBirth = ParseDate(record.DateOfBirth),
                        Telephone = record.Telephone,
                        Mobile = record.Mobile,
                        Address = record.Address,
                        Address2 = record.Address2,
                        Postcode = record.Postcode,
                        EmailHome = record.EmailHome,
                        StartDate = ParseDate(record.StartDate)
                    };

                    employees.Add(employee);
                }

                return (employees, employees.Count);
            });
        }

        private DateTime? ParseDate(string dateStr)
        {
            if (string.IsNullOrEmpty(dateStr))
                return null;

            // Try to parse different date formats
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                return date;

            if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                return date;

            if (DateTime.TryParse(dateStr, out date))
                return date;

            return null;
        }
    }
}

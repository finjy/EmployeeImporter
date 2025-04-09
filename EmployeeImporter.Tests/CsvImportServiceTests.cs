using EmployeeImporter.Infrastructure.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeImporter.Tests
{
    public class CsvImportServiceTests
    {
        [Fact]
        public async Task ImportEmployeesFromCsvAsync_ValidCsv_ReturnsCorrectEmployees()
        {
            // Arrange
            var csvContent = @"Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date
              EMP001,John,Doe,1980-01-01,1234567890,9876543210,123 Main St,Apt 4B,12345,john.doe@example.com,2020-01-15";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvImportService();

            // Act
            var result = await service.ImportEmployeesFromCsvAsync(stream);

            // Assert
            Assert.NotNull(result.employees);
            Assert.Single(result.employees);
            Assert.Equal(1, result.processedCount);
            Assert.Equal("EMP001", result.employees[0].PayrollNumber);
            Assert.Equal("John", result.employees[0].Forenames);
            Assert.Equal("Doe", result.employees[0].Surname);
            Assert.Equal(new DateTime(1980, 1, 1), result.employees[0].DateOfBirth);
            Assert.Equal("1234567890", result.employees[0].Telephone);
            Assert.Equal("9876543210", result.employees[0].Mobile);
            Assert.Equal("123 Main St", result.employees[0].Address);
            Assert.Equal("Apt 4B", result.employees[0].Address2);
            Assert.Equal("12345", result.employees[0].Postcode);
            Assert.Equal("john.doe@example.com", result.employees[0].EmailHome);
            Assert.Equal(new DateTime(2020, 1, 15), result.employees[0].StartDate);
        }

        [Fact]
        public async Task ImportEmployeesFromCsvAsync_EmptyCsv_ReturnsEmptyList()
        {
            // Arrange
            var csvContent = "Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvImportService();

            // Act
            var result = await service.ImportEmployeesFromCsvAsync(stream);

            // Assert
            Assert.NotNull(result.employees);
            Assert.Empty(result.employees);
            Assert.Equal(0, result.processedCount);
        }

        [Fact]
        public async Task ImportEmployeesFromCsvAsync_HandlesMultipleDateFormats()
        {
            // Arrange
            var csvContent = @"Personnel_Records.Payroll_Number,Personnel_Records.Forenames,Personnel_Records.Surname,Personnel_Records.Date_of_Birth,Personnel_Records.Telephone,Personnel_Records.Mobile,Personnel_Records.Address,Personnel_Records.Address_2,Personnel_Records.Postcode,Personnel_Records.EMail_Home,Personnel_Records.Start_Date
EMP001,John,Doe,01/01/1980,1234567890,9876543210,123 Main St,Apt 4B,12345,john.doe@example.com,2020-01-15
EMP002,Jane,Smith,1985-05-15,9876543210,1234567890,456 Oak St,Unit 7C,54321,jane.smith@example.com,15/06/2019";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            var service = new CsvImportService();

            // Act
            var result = await service.ImportEmployeesFromCsvAsync(stream);

            // Assert
            Assert.NotNull(result.employees);
            Assert.Equal(2, result.employees.Count);
            Assert.Equal(2, result.processedCount);
            Assert.Equal(new DateTime(1980, 1, 1), result.employees[0].DateOfBirth);
            Assert.Equal(new DateTime(1985, 5, 15), result.employees[1].DateOfBirth);
            Assert.Equal(new DateTime(2020, 1, 15), result.employees[0].StartDate);
            Assert.Equal(new DateTime(2019, 6, 15), result.employees[1].StartDate);
        }
    }
}
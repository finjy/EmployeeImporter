using EmployeeImporter.Core.Application.Services;
using EmployeeImporter.Core.Domain.Entities;
using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Core.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeImporter.Tests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepository;
        private readonly Mock<ICsvImportService> _mockCsvService;
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _mockRepository = new Mock<IEmployeeRepository>();
            _mockCsvService = new Mock<ICsvImportService>();
            _service = new EmployeeService(_mockRepository.Object, _mockCsvService.Object);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_DefaultSort_ReturnsSortedBySurname()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Surname = "Brown", Forenames = "Charlie" },
                new Employee { Id = 2, Surname = "Adams", Forenames = "Alice" },
                new Employee { Id = 3, Surname = "Davis", Forenames = "Bob" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);

            // Act
            var result = await _service.GetAllEmployeesAsync("surname_asc");

            // Assert
            var sortedEmployees = result.ToList();
            Assert.Equal(3, sortedEmployees.Count);
            Assert.Equal("Adams", sortedEmployees[0].Surname);
            Assert.Equal("Brown", sortedEmployees[1].Surname);
            Assert.Equal("Davis", sortedEmployees[2].Surname);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_DescendingSort_ReturnsSortedBySurnameDescending()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Surname = "Brown", Forenames = "Charlie" },
                new Employee { Id = 2, Surname = "Adams", Forenames = "Alice" },
                new Employee { Id = 3, Surname = "Davis", Forenames = "Bob" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);

            // Act
            var result = await _service.GetAllEmployeesAsync("surname_desc");

            // Assert
            var sortedEmployees = result.ToList();
            Assert.Equal(3, sortedEmployees.Count);
            Assert.Equal("Davis", sortedEmployees[0].Surname);
            Assert.Equal("Brown", sortedEmployees[1].Surname);
            Assert.Equal("Adams", sortedEmployees[2].Surname);
        }

        [Fact]
        public async Task SearchEmployeesAsync_ValidSearchTerm_ReturnsFilteredResults()
        {
            // Arrange
            var searchTerm = "Smith";
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Surname = "Smith", Forenames = "John" },
                new Employee { Id = 2, Surname = "Johnson", Forenames = "Smith" },
                new Employee { Id = 3, Surname = "Davis", Forenames = "Bob" }
            };

            _mockRepository.Setup(repo => repo.SearchAsync(searchTerm))
                .ReturnsAsync(employees.Where(e =>
                    e.Surname.Contains(searchTerm) ||
                    e.Forenames.Contains(searchTerm)));

            // Act
            var result = await _service.SearchEmployeesAsync(searchTerm, "surname_asc");

            // Assert
            var filteredEmployees = result.ToList();
            Assert.Equal(2, filteredEmployees.Count);
            Assert.Contains(filteredEmployees, e => e.Id == 1);
            Assert.Contains(filteredEmployees, e => e.Id == 2);
        }

        [Fact]
        public async Task ImportEmployeesFromCsvAsync_ValidFile_AddsToRepositoryAndReturnsCount()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Surname = "Doe", Forenames = "John" },
                new Employee { Id = 2, Surname = "Smith", Forenames = "Jane" }
            };

            var stream = new MemoryStream();

            _mockCsvService.Setup(csv => csv.ImportEmployeesFromCsvAsync(stream))
                .ReturnsAsync((employees, 2));

            // Act
            var result = await _service.ImportEmployeesFromCsvAsync(stream);

            // Assert
            Assert.Equal(2, result);
            _mockRepository.Verify(repo => repo.AddRangeAsync(employees), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ValidEmployee_UpdatesRepositoryAndReturnsEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Surname = "Doe",
                Forenames = "John Updated"
            };

            // Act
            var result = await _service.UpdateEmployeeAsync(employee);

            // Assert
            Assert.Equal(employee, result);
            _mockRepository.Verify(repo => repo.Update(employee), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}

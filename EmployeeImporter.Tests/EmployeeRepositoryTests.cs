using EmployeeImporter.Core.Domain.Entities;
using EmployeeImporter.Infrastructure.Data.Context;
using EmployeeImporter.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeImporter.Tests
{
    public class EmployeeRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public EmployeeRepositoryTests()
        {
            // Create a unique database name for each test to avoid conflicts
            var dbName = $"EmployeeImporter_Test_{Guid.NewGuid()}";
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            // Seed the database
            using (var context = new ApplicationDbContext(_options))
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        Id = 1,
                        PayrollNumber = "EMP001",
                        Forenames = "John",
                        Surname = "Doe",
                        DateOfBirth = new DateTime(1980, 1, 1),
                        Telephone = "1234567890",
                        EmailHome = "john.doe@example.com",
                        Address = "123 Main St",
                        Address2 = "Apt 1",
                        Mobile = "9876543210",
                        Postcode = "12345"
                    },
                    new Employee
                    {
                        Id = 2,
                        PayrollNumber = "EMP002",
                        Forenames = "Jane",
                        Surname = "Smith",
                        DateOfBirth = new DateTime(1985, 5, 15),
                        Telephone = "0987654321",
                        EmailHome = "jane.smith@example.com",
                        Address = "456 Oak St",
                        Address2 = "Suite 2",
                        Mobile = "1234567890",
                        Postcode = "54321"
                    },
                    new Employee
                    {
                        Id = 3,
                        PayrollNumber = "EMP003",
                        Forenames = "Bob",
                        Surname = "Johnson",
                        DateOfBirth = new DateTime(1990, 10, 20),
                        Telephone = "5555555555",
                        EmailHome = "bob.johnson@example.com",
                        Address = "789 Pine St",
                        Address2 = "Unit 3",
                        Mobile = "5555555555",
                        Postcode = "67890"
                    }
                );
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEmployees()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new EmployeeRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            var employees = result.ToList();
            Assert.Equal(3, employees.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsEmployee()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new EmployeeRepository(context);

            // Act
            var employee = await repository.GetByIdAsync(2);

            // Assert
            Assert.NotNull(employee);
            Assert.Equal("Jane", employee.Forenames);
            Assert.Equal("Smith", employee.Surname);
        }

        [Fact]
        public async Task SearchAsync_ValidTerm_ReturnsMatchingEmployees()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new EmployeeRepository(context);

            // Act
            var result = await repository.SearchAsync("John");

            // Assert
            var employees = result.ToList();
            Assert.Equal(2, employees.Count); // Should match "John" and "Johnson"
            Assert.Contains(employees, e => e.Id == 1);
            Assert.Contains(employees, e => e.Id == 3);
        }

        [Fact]
        public async Task AddAsync_NewEmployee_AddsToDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new EmployeeRepository(context);

            var newEmployee = new Employee
            {
                PayrollNumber = "EMP004",
                Forenames = "Alice",
                Surname = "Brown",
                DateOfBirth = new DateTime(1995, 3, 10),
                Telephone = "1112223333",
                EmailHome = "alice.brown@example.com",
                Address = "789 Pine St",
                Address2 = "Apt 101",
                Mobile = "5556667777",
                Postcode = "67890"
            };

            // Act
            await repository.AddAsync(newEmployee);
            await repository.SaveChangesAsync();

            // Get a fresh context to verify changes were saved
            using var verifyContext = new ApplicationDbContext(_options);
            var savedEmployee = await verifyContext.Employees.Where(e => e.PayrollNumber == "EMP004").FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(savedEmployee);
            Assert.Equal("Alice", savedEmployee.Forenames);
            Assert.Equal("Brown", savedEmployee.Surname);
        }

        [Fact]
        public async Task Update_ExistingEmployee_UpdatesInDatabase()
        {
            // Arrange
            using var context = new ApplicationDbContext(_options);
            var repository = new EmployeeRepository(context);

            // Get existing employee
            var employee = await context.Employees.FindAsync(3);
            employee.Forenames = "Robert"; // Change from "Bob" to "Robert"
            employee.Telephone = "9999999999"; // Update phone

            // Act
            repository.Update(employee);
            await repository.SaveChangesAsync();

            // Get a fresh context to verify changes were saved
            using var verifyContext = new ApplicationDbContext(_options);
            var updatedEmployee = await verifyContext.Employees.FindAsync(3);

            // Assert
            Assert.Equal("Robert", updatedEmployee.Forenames);
            Assert.Equal("9999999999", updatedEmployee.Telephone);
        }
    }
}
using EmployeeImporter.Controllers;
using EmployeeImporter.Core.Application.Interfaces;
using EmployeeImporter.Core.Domain.Entities;
using EmployeeImporter.Controllers;
using EmployeeImporter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeImporter.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _controller = new HomeController(_mockEmployeeService.Object);

            // Setup TempData
            _controller.TempData = new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(
                new DefaultHttpContext(),
                Mock.Of<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider>());
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task GetEmployees_ReturnsJsonResult_WithEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Surname = "Doe", Forenames = "John" },
                new Employee { Id = 2, Surname = "Smith", Forenames = "Jane" }
            };

            _mockEmployeeService.Setup(s => s.GetAllEmployeesAsync("surname_asc"))
                .ReturnsAsync(employees);

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var resultValue = Assert.IsAssignableFrom<IEnumerable<Employee>>(jsonResult.Value);
            Assert.Equal(2, ((List<Employee>)resultValue).Count);
        }

        [Fact]
        public async Task Import_WithValidFile_ImportsEmployeesAndRedirects()
        {
            // Arrange
            var fileContent = Encoding.UTF8.GetBytes("test csv content");
            var formFile = new FormFile(
                baseStream: new MemoryStream(fileContent),
                baseStreamOffset: 0,
                length: fileContent.Length,
                name: "file",
                fileName: "employees.csv");

            _mockEmployeeService.Setup(s => s.ImportEmployeesFromCsvAsync(It.IsAny<Stream>()))
                .ReturnsAsync(5); // 5 records imported

            // Act
            var result = await _controller.Import(formFile);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Imported 5 records", _controller.TempData["Success"]);
        }

        [Fact]
        public async Task Import_WithEmptyFile_ReturnsErrorMessage()
        {
            // Arrange
            var formFile = new FormFile(
                baseStream: new MemoryStream(),
                baseStreamOffset: 0,
                length: 0, // Empty file
                name: "file",
                fileName: "empty.csv");

            // Act
            var result = await _controller.Import(formFile);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("File is not selected or empty", _controller.TempData["Error"]);
        }

        [Fact]
        public async Task Import_WithNonCsvFile_ReturnsErrorMessage()
        {
            // Arrange
            var fileContent = Encoding.UTF8.GetBytes("fake content");
            var formFile = new FormFile(
                baseStream: new MemoryStream(fileContent),
                baseStreamOffset: 0,
                length: fileContent.Length,
                name: "file",
                fileName: "employees.txt"); // Not a CSV file

            // Act
            var result = await _controller.Import(formFile);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("File format must be CSV", _controller.TempData["Error"]);
        }

        [Fact]
        public async Task Update_WithValidEmployee_ReturnsOkResult()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Surname = "Doe",
                Forenames = "John Updated"
            };

            _mockEmployeeService.Setup(s => s.UpdateEmployeeAsync(It.IsAny<Employee>()))
                .ReturnsAsync(employee);

            // Act
            var result = await _controller.Update(employee);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultValue = Assert.IsType<Employee>(okResult.Value);
            Assert.Equal("John Updated", resultValue.Forenames);
        }

        [Fact]
        public async Task Update_WithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Surname", "Required");

            // Act
            var result = await _controller.Update(new Employee());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}

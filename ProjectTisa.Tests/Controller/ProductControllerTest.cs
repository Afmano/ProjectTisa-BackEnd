using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectTisa.Controllers.BusinessControllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.EF;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class ProductControllerTest
    {
        private readonly ILogger<ProductController> _logger = A.Fake<ILogger<ProductController>>();
        #region empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Get(idToRequest);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryId_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            int categoryIdToRequest = 1;
            // Act
            var result = await controller.GetAllByCategory(categoryId: categoryIdToRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryName_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            string categoryNameToRequest = "test";
            // Act
            var result = await controller.GetAllByCategory(categoryName: categoryNameToRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void Delete_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Delete(idToRequest);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void Update_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Name = "", PhotoPath = "", CategoryId = 1, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        #endregion
        #region filled
        [Fact]
        public async void GetAll_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetById_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(entity.Id);
            var okObjectResultResult = result.Result as OkObjectResult;
            var resultProduct = okObjectResultResult?.Value as Product;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResultResult!.Value.Should().NotBeNull();
            okObjectResultResult!.Value.Should().BeOfType(typeof(Product));
            resultProduct!.Name.Should().Be(product.Name);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryId_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllByCategory(categoryId: entity.Category.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryName_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllByCategory(categoryName: entity.Category.Name);
            var okObjectResult = result.Result as OkObjectResult;
            var products = okObjectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(entity.Id);
            var okObjectResultResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResultResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResultResult!.Value.Should().NotBeNull();
            okObjectResultResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
        }
        [Fact]
        public async void Update_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            ProductController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var categoryEntity = dbContext.Categories.Add(category).Entity;
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(entity.Id, new() { Name = "", PhotoPath = "", CategoryId = categoryEntity.Id, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var okObjectResultResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResultResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResultResult!.Value.Should().NotBeNull();
            okObjectResultResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
        }
        [Fact]
        public async void Create_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            ProductController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            var categoryEntity = dbContext.Categories.Add(category).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.Create(new() { Name = "", PhotoPath = "", CategoryId = categoryEntity.Id, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var okObjectResultResult = result.Result as CreatedResult;
            var resultMessage = okObjectResultResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            okObjectResultResult!.Value.Should().NotBeNull();
            okObjectResultResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Created);
        }
        [Fact]
        public async void Update_WithoutCat_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            ProductController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            var entity = dbContext.Products.Add(product).Entity;
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(entity.Id, new() { Name = "", PhotoPath = "", CategoryId = 0, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var notFoundResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundResult!.Value.Should().NotBeNull();
            notFoundResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void Create_WithoutCat_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            ProductController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Name = "", PhotoPath = "", CategoryId = 0, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var notFoundResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundResult!.Value.Should().NotBeNull();
            notFoundResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductController controller = new(_logger, dbContext);
            int idToRequest = -1;
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(idToRequest);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult!.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        #endregion
    }
}

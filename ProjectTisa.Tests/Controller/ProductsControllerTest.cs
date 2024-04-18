﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.EF;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class ProductsControllerTest
    {
        private readonly ILogger<ProductsController> _logger = new Mock<ILogger<ProductsController>>().Object;
        #region Empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Get(idToRequest);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryId_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            int categoryIdToRequest = 1;
            // Act
            var result = await controller.GetAllByCategory(categoryId: categoryIdToRequest);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryName_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            string categoryNameToRequest = "test";
            // Act
            var result = await controller.GetAllByCategory(categoryName: categoryNameToRequest);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().Be(0);
        }
        [Fact]
        public async void Delete_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Delete(idToRequest);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void Update_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Name = "", PhotoPath = "", CategoryId = 1, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        #endregion
        #region Filled
        #region OkResult
        [Fact]
        public async void GetAll_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetById_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(product.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultProduct = objectResult?.Value as Product;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(Product));
            resultProduct!.Name.Should().Be(product.Name);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryId_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllByCategory(categoryId: product.Category.Id);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetAllByCategory_ByCategoryName_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllByCategory(categoryName: product.Category.Name);
            var objectResult = result.Result as OkObjectResult;
            var products = objectResult?.Value as List<Product>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Product>));
            products!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(product.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Products.Count().Should().Be(0);
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
            ProductsController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category categoryOld = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            string newCatName = "testNew";
            Category categoryNew = new() { Name = newCatName, PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = categoryOld, IsAvailable = true, Price = 1 };
            string newEntityName = "testNew";
            // Act
            dbContext.Add(product);
            dbContext.Add(categoryNew);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(product.Id, new() { Name = newEntityName, PhotoPath = "", CategoryId = categoryNew.Id, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Products.Find(product.Id)!.Category.Id.Should().Be(categoryNew.Id);
            dbContext.Products.Find(product.Id)!.Category.Name.Should().Be(newCatName);
            dbContext.Products.Find(product.Id)!.Name.Should().Be(newEntityName);
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
            ProductsController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await controller.Create(new() { Name = "", PhotoPath = "", CategoryId = category.Id, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var objectResult = result.Result as CreatedResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.Products.Count());
            dbContext.Products.Count().Should().NotBe(0);
        }
        #endregion
        #region NotFoundResult
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
            ProductsController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(product.Id, new() { Name = "", PhotoPath = "", CategoryId = 0, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
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
            ProductsController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Name = "", PhotoPath = "", CategoryId = 0, IsAvailable = true, Price = 1 });//validation attributes ignored here
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ProductsController controller = new(_logger, dbContext);
            int idToRequest = -1;
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(idToRequest);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        #endregion
        #endregion
    }
}

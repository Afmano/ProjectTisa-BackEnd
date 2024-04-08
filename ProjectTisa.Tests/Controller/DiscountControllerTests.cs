using FluentAssertions;
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
    public class DiscountControllerTests
    {
        private readonly ILogger<DiscountController> _logger = new Mock<ILogger<DiscountController>>().Object;
        #region Empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var discounts = objectResult?.Value as List<Discount>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Discount>));
            discounts!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
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
        public async void Delete_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
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
            DiscountController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Name = "", DiscountPercent= 0.1m });//validation attributes ignored here
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
            DiscountController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester") };
            // Act
            dbContext.Add(discount);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var discounts = objectResult?.Value as List<Discount>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Discount>));
            discounts!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetById_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester") };
            // Act
            dbContext.Add(discount);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(discount.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultDiscount = objectResult?.Value as Discount;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(Discount));
            resultDiscount!.Name.Should().Be(discount.Name);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester") };
            // Act
            dbContext.Add(discount);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(discount.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Discounts.Count().Should().Be(0);
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
            DiscountController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester") };
            // Act
            dbContext.Add(discount);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(discount.Id, new() { Name = "", DiscountPercent= 0.1m });//validation attributes ignored here
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
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
            DiscountController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Name = "", DiscountPercent= 0.1m });//validation attributes ignored here
            var objectResult = result.Result as CreatedResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.Discounts.Count());
            dbContext.Discounts.Count().Should().NotBe(0);
        }
        [Fact]
        public async void Update_WithProductIds_ReturnOk()
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
            DiscountController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product productOld = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Product productNew = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 2 };
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester"), Products = [productOld] };
            string newName = "testNew";
            // Act
            dbContext.Add(discount);
            dbContext.Add(productNew);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(discount.Id, new() { Name = newName, DiscountPercent = 0.1m, ProductIds = [productNew.Id] });//validation attributes ignored here
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Discounts.Find(discount.Id)!.Products.First().Id.Should().Be(productNew.Id);
            dbContext.Discounts.Find(discount.Id)!.Name.Should().Be(newName);
        }
        [Fact]
        public async void Create_WithProductIds_ReturnOk()
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
            DiscountController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            // Act
            dbContext.Add(product);
            var result = await controller.Create(new() { Name = "", DiscountPercent = 0.1m, ProductIds = [product.Id] });//validation attributes ignored here
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
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DiscountController controller = new(_logger, dbContext);
            int idToRequest = -1;
            Discount discount = new() { Name = "", DiscountPercent = 0.1m, EditInfo = new("tester") };
            // Act
            dbContext.Add(discount);
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
    }
}

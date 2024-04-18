using Microsoft.Extensions.Logging;
using ProjectTisa.Tests.Contexts;
using ProjectTisa.Controllers.GeneralData.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.EF;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Principal;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Responses;

namespace ProjectTisa.Tests.Controller
{
    public class CategoriesControllerTests
    {
        private readonly ILogger<CategoriesController> _logger = new Mock<ILogger<CategoriesController>>().Object;
        #region Empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            CategoriesController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var categories = objectResult?.Value as List<Category>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Category>));
            categories!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            CategoriesController controller = new(_logger, dbContext);
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
            CategoriesController controller = new(_logger, dbContext);
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
            CategoriesController controller = new(_logger, dbContext);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Name = "", PhotoPath = "" });//validation attributes ignored here
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
            CategoriesController controller = new(_logger, dbContext);
            PaginationRequest paginationRequest = new();
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var categories = objectResult?.Value as List<Category>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Category>));
            categories!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetById_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            CategoriesController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(category.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultCategory = objectResult?.Value as Category;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(Category));
            resultCategory!.Name.Should().Be(category.Name);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            CategoriesController controller = new(_logger, dbContext);
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(category.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Categories.Count().Should().Be(0);
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
            CategoriesController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(category.Id, new() { Name = "", PhotoPath = "" });//validation attributes ignored here
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
            CategoriesController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Name = "", PhotoPath = "" });//validation attributes ignored here
            var objectResult = result.Result as CreatedResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.Categories.Count());
            dbContext.Categories.Count().Should().NotBe(0);
        }
        [Fact]
        public async void Update_WithParentCat_ReturnOk()
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
            CategoriesController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category categoryOldParent = new() { Name = "parent", PhotoPath = "", EditInfo = new("tester")};
            string newCatName = "testNew";
            Category categoryNewParent = new() { Name = newCatName, PhotoPath = "", EditInfo = new("tester")};
            Category category = new() { Name = "", PhotoPath = "", ParentCategory = categoryOldParent, EditInfo = new("tester")};
            string newEntityName = "testNew";
            // Act
            dbContext.AddRange(category, categoryNewParent);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(category.Id, new() { Name = newEntityName, PhotoPath = "", ParentCategoryId= categoryNewParent.Id });//validation attributes ignored here
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Categories.Find(category.Id)!.ParentCategory!.Id.Should().Be(categoryNewParent.Id);
            dbContext.Categories.Find(category.Id)!.ParentCategory!.Name.Should().Be(newCatName);
            dbContext.Categories.Find(category.Id)!.Name.Should().Be(newEntityName);
        }
        [Fact]
        public async void Create_WithParentCat_ReturnOk()
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
            CategoriesController controller = new(_logger, dbContext) { ControllerContext = controllerContext };
            Category categoryParent = new() { Name = "parent", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(categoryParent);
            var result = await controller.Create(new() { Name = "", PhotoPath = "", ParentCategoryId = categoryParent.Id });//validation attributes ignored here
            var objectResult = result.Result as CreatedResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.Categories.Count());
            dbContext.Categories.Count().Should().NotBe(0);
        }
        #endregion
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            CategoriesController controller = new(_logger, dbContext);
            int idToRequest = -1;
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(category);
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

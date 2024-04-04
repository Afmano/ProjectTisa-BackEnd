using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.EF;
using ProjectTisa.Models;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class NotificationControllerTests
    {
        private readonly ILogger<NotificationController> _logger = new Mock<ILogger<NotificationController>>().Object;
        private readonly IAuthorizationService _authorizationService = new Mock<IAuthorizationService>().Object;
        #region Empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var notifications = okObjectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Notification>));
            notifications!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetAllNotifsByCurrentUser_ReturnEmptyList()
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
            NotificationController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "test", Username = "tester" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllNotifsByCurrentUser(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var notifications = okObjectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Notification>));
            notifications!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
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
        public async void Delete_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
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
            NotificationController controller = new(_logger, dbContext, _authorizationService);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Caption = "", Message = "" });//validation attributes ignored here
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
        #region Filled
        #region OkResult
        [Fact]
        public async void GetAll_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var notifications = okObjectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Notification>));
            notifications!.Count.Should().NotBe(0);
        }
        [Fact]
        public async void GetAllNotifsByCurrentUser_ReturnOk()
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
            NotificationController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "test", Username = "tester", Notifications = [new() { Caption = "test", Message = "test", EditInfo = new("tester") }] };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllNotifsByCurrentUser(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var notifications = okObjectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Notification>));
            notifications!.Count.Should().Be(1);
        }
        [Fact]
        public async void GetById_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            Mock<IAuthorizationService> customMockAuthorizeService = new();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            customMockAuthorizeService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>())).ReturnsAsync(AuthorizationResult.Failed);
            NotificationController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
            User user = new() { Email = "test", Username = "tester" };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester"), Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(notification.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var resultNotification = okObjectResult?.Value as Notification;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(Notification));
            resultNotification!.Caption.Should().Be(notification.Caption);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(notification.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Notifications.Count().Should().Be(0);
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
            NotificationController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "test", Username = "tester" };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester"), Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(notification.Id, new() { Caption = "", Message = "" });//validation attributes ignored here
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            notification.Users.First().Should().Be(user);
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
            NotificationController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Caption = "", Message = "" });//validation attributes ignored here
            var createdResult = result.Result as CreatedResult;
            var resultMessage = createdResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            createdResult!.Value.Should().NotBeNull();
            createdResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Created);
            dbContext.Notifications.Count().Should().Be(1);
        }
        #endregion
        #region NotFound
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationController controller = new(_logger, dbContext, _authorizationService);
            int idToRequest = -1;
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
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
        [Fact]
        public async void Update_NegativeId_ReturnNotFound()
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
            NotificationController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            int idToRequest = -1;
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(idToRequest, new() { Caption = "", Message = "" });//validation attributes ignored here
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
        [Fact]
        public async void GetById_ReturnForbid()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            Mock<IAuthorizationService> customMockAuthorizeService = new();
            customMockAuthorizeService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>())).ReturnsAsync(AuthorizationResult.Failed);
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("wrongTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            NotificationController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
            User user = new() { Email = "test", Username = "tester" };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester"), Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(notification.Id);
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(ForbidResult));
        }
        #endregion
    }
}

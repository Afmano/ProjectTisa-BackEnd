using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
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
        private readonly ILogger<NotificationsController> _logger = new Mock<ILogger<NotificationsController>>().Object;
        private readonly IAuthorizationService _authorizationService = new Mock<IAuthorizationService>().Object;
        #region Empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var notifications = objectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Notification>));
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "test", Username = "tester" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllNotifsByCurrentUser(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var notifications = objectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Notification>));
            notifications!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
            int idToRequest = 1;
            // Act
            var result = await controller.Update(idToRequest, new() { Caption = "", Message = "" });//validation attributes ignored here
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var notifications = objectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Notification>));
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "test", Username = "tester", Notifications = [new() { Caption = "test", Message = "test", EditInfo = new("tester") }] };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetAllNotifsByCurrentUser(paginationRequest);
            var objectResult = result.Result as OkObjectResult;
            var notifications = objectResult?.Value as List<Notification>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(List<Notification>));
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
            NotificationsController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
            User user = new() { Email = "test", Username = "tester" };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester"), Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(notification.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultNotification = objectResult?.Value as Notification;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(Notification));
            resultNotification!.Caption.Should().Be(notification.Caption);
        }
        [Fact]
        public async void Delete_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Delete(notification.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "test", Username = "tester" };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester"), Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(notification.Id, new() { Caption = "", Message = "" });//validation attributes ignored here
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            // Act
            var result = await controller.Create(new() { Caption = "", Message = "" });//validation attributes ignored here
            var objectResult = result.Result as CreatedResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.Notifications.Count());
            dbContext.Notifications.Count().Should().NotBe(0);
        }
        #endregion
        #region NotFound
        [Fact]
        public async void Delete_NegativeId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            NotificationsController controller = new(_logger, dbContext, _authorizationService);
            int idToRequest = -1;
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            // Act
            dbContext.Add(notification);
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
            NotificationsController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            Notification notification = new() { Caption = "", Message = "", EditInfo = new("tester") };
            int idToRequest = -1;
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(idToRequest, new() { Caption = "", Message = "" });//validation attributes ignored here
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
            NotificationsController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
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

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.BusinessControllers.RoleControllers;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.EF;
using ProjectTisa.Models;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Models.Enums;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class ManagerControllerTests //tests for /LoadFile and /GetFiles skipped due to external API relation.
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        private readonly IOptions<RouteConfig> _config = Options.Create<RouteConfig>(new()
        {
            ApplicationName = "",
            AuthData = null,
            CurrentHost = "",
            Version = "",
            ExternalStorage = new() { Auth = "", GetUrl = "", PostUrl = "" },
            SmtpData = null
        });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        #region Empty
        [Fact]
        public async void SendNotificationByRole_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ManagerController controller = new(dbContext, _config);
            RoleType roleToNotif = RoleType.User;
            int idToRequest = 1;
            // Act
            var result = await controller.SendNotificationByRole(idToRequest, roleToNotif);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult?.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void SendNotificationByUsername_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ManagerController controller = new(dbContext, _config);
            string usernameToNotif = "test";
            int idToRequest = 1;
            // Act
            var result = await controller.SendNotificationByUsername(idToRequest, usernameToNotif);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult?.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void SendNotificationByEmail_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ManagerController controller = new(dbContext, _config);
            string emailToNotif = "test";
            int idToRequest = 1;
            // Act
            var result = await controller.SendNotificationByEmail(idToRequest, emailToNotif);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult?.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        [Fact]
        public async void DetachNotification_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            ManagerController controller = new(dbContext, _config);
            int idToRequest = 1;
            // Act
            var result = await controller.DetachNotification(idToRequest);
            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            var resultMessage = notFoundObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            notFoundObjectResult?.Value.Should().NotBeNull();
            notFoundObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
        }
        #endregion
        #region Filled
        #region OkObject
        [Fact]
        public async void SendNotificationByRole_ReturnOk()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            RoleType roleToNotif = RoleType.User;
            List<User> users = [
                new() { Email="1", Username="1", Role = RoleType.User},
                new() { Email="2", Username="2", Role = RoleType.User},
                new() { Email="3", Username="3", Role = RoleType.Manager},
                new() { Email="4", Username="4", Role = RoleType.Admin},
                new() { Email="5", Username="5", Role = RoleType.SuperAdmin}
            ];
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.AddRange(users);
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByRole(notification.Id, roleToNotif);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Notifications.Find(notification.Id)!.Users.Count.Should().Be(users.Where(u => u.Role == roleToNotif).Count());
        }
        [Fact]
        public async void SendNotificationByUsername_ReturnOk()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            string usernameToNotif = "test";
            User user = new() { Email = "", Username = usernameToNotif };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.Add(user);
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByUsername(notification.Id, usernameToNotif);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Notifications.Find(notification.Id)!.Users.First().Should().Be(user);
        }
        [Fact]
        public async void SendNotificationByEmail_ReturnOk()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            string emailToNotif = "test";
            User user = new() { Email = emailToNotif, Username = "" };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.Add(user);
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByEmail(notification.Id, emailToNotif);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Notifications.Find(notification.Id)!.Users.First().Should().Be(user);
        }
        [Fact]
        public async void DetachNotification_ReturnOk()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "" };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "", Users = [user] };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.DetachNotification(notification.Id);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Notifications.Find(notification.Id)!.Users.Count.Should().Be(0);
        }
        #endregion
        #region NotFound
        [Fact]
        public async void SendNotificationByRole_NoUsers_ReturnNotFound()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            RoleType roleToNotif = RoleType.User;
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByRole(notification.Id, roleToNotif);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullContext);
        }
        [Fact]
        public async void SendNotificationByUsername_NoUser_ReturnNotFound()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            string usernameToNotif = "test";
            User user = new() { Email = "", Username = "randomUser" };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.Add(user);
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByUsername(notification.Id, usernameToNotif);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
            dbContext.Notifications.Find(notification.Id)!.Users.Count.Should().Be(0);
        }
        [Fact]
        public async void SendNotificationByEmail_NoUser_ReturnNotFound()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            string emailToNotif = "test";
            User user = new() { Email = "", Username = "randomUser" };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "" };
            // Act
            dbContext.Add(user);
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.SendNotificationByEmail(notification.Id, emailToNotif);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
            dbContext.Notifications.Find(notification.Id)!.Users.Count.Should().Be(0);
        }


        [Fact]
        public async void DetachNotification_NegativeId_ReturnNotFound()
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
            ManagerController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "" };
            Notification notification = new() { Caption = "", EditInfo = new("tester"), Message = "", Users = [user] };
            int requestId = -1;
            // Act
            dbContext.Add(notification);
            await dbContext.SaveChangesAsync();
            var result = await controller.DetachNotification(requestId);
            var objectResult = result.Result as NotFoundObjectResult;
            var resultMessage = objectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(NotFoundObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.NotFoundNullEntity);
            dbContext.Notifications.Find(notification.Id)!.Users.Count.Should().Be(1);
        }
        #endregion
        #endregion
    }
}

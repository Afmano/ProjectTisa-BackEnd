using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.BusinessControllers.RoleControllers;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.EF;
using ProjectTisa.Models;
using ProjectTisa.Models.Enums;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class AdminControllerTests
    {
        #region Empty
        [Fact]
        public async void SetRoleToUser_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AdminController controller = new(dbContext);
            RoleType roleToSet = RoleType.Manager;
            int idToRequest = 1;
            // Act
            var result = await controller.SetRole(idToRequest, roleToSet);
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
        #region OkResult
        [Fact]
        public async void SetRoleToUser_UserToManager_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("otherTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test", Role = RoleType.User };
            RoleType roleToSet = RoleType.Manager;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleToSet);
        }
        [Fact]
        public async void SetRoleToUser_ManagerToUser_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("otherTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test", Role = RoleType.Manager };
            RoleType roleToSet = RoleType.User;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleToSet);
        }
        #endregion
        #region NotFound
        [Fact]
        public async void SetRoleToUser_NegativeId_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("otherTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test", Role = RoleType.User };
            int idToRequest = -1;
            RoleType roleToSet = RoleType.Manager;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(idToRequest, roleToSet);
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
        #region BadRequest
        [Fact]
        public async void SetRoleToUser_UserToManager_SameUser_ReturnBadRequest()
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
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            RoleType roleOld = RoleType.User;
            User user = new() { Username = "tester", Email = "test", Role = roleOld };
            RoleType roleToSet = RoleType.Manager;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult?.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleOld);
        }
        [Fact]
        public async void SetRoleToUser_ManagerToUser_SameUser_ReturnBadRequest()
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
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            RoleType roleOld = RoleType.Manager;
            User user = new() { Username = "tester", Email = "test", Role = roleOld };
            RoleType roleToSet = RoleType.User;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult?.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleOld);
        }
        [Fact]
        public async void SetRoleToUser_UserToAdmin_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("otherTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            RoleType roleOld = RoleType.User;
            User user = new() { Username = "tester", Email = "test", Role = roleOld };
            RoleType roleToSet = RoleType.Admin;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult?.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleOld);
        }
        [Fact]
        public async void SetRoleToUser_AdminToUser_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("otherTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            AdminController controller = new(dbContext) { ControllerContext = controllerContext };
            RoleType roleOld = RoleType.Admin;
            User user = new() { Username = "tester", Email = "test", Role = roleOld };
            RoleType roleToSet = RoleType.User;
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.SetRole(user.Id, roleToSet);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult?.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
            dbContext.Users.Find(user.Id)!.Role.Should().Be(roleOld);
        }
        #endregion
        #endregion
    }
}

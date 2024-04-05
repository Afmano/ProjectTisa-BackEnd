using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.UserRelatedControllers;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.EF;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using ProjectTisa.Tests.Contexts;
using System.Security.Claims;
using System.Security.Principal;

namespace ProjectTisa.Tests.Controller
{
    public class UserControllerTests
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        private readonly IOptions<RouteConfig> _config = Options.Create<RouteConfig>(new()
        {
            ApplicationName = "",
            AuthData = new()
            {
                Audience = "",
                ExpirationTime = new(),
                HashAlgorithmOID = "1.3.14.3.2.26",
                Issuer = "",
                IssuerSigningKey = "test",
                IterationCount = 1,
                SaltSize = 1
            },
            CurrentHost = "",
            Version = "",
            ExternalStorage = null,
            SmtpData = null
        });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        #region Error
        [Fact]
        public async void GetUser_WrongClaim_ReturnError()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            UserController controller = new(dbContext, _config);
            // Act & Assert
            (await Assert.ThrowsAsync<ControllerException>(controller.GetUser)).Message.Should().Be(ResAnswers.WrongJWT);
        }
        [Fact]
        public async void GetUser_UserNotExist_ReturnError()
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
            UserController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            // Act & Assert
            (await Assert.ThrowsAsync<ControllerException>(controller.GetUser)).Message.Should().Be(ResAnswers.UserNotFound);
        }
        [Fact]
        public async void ChangePassword_WrongClaim_ReturnError()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            UserController controller = new(dbContext, _config);
            string newPassword = "testPass123";
            // Act & Assert
            (await Assert.ThrowsAsync<ControllerException>(() => controller.ChangePassword(newPassword))).Message.Should().Be(ResAnswers.WrongJWT);
        }
        [Fact]
        public async void ChangePassword_UserNotExist_ReturnError()
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
            UserController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            string newPassword = "testPass123";
            // Act & Assert
            (await Assert.ThrowsAsync<ControllerException>(() => controller.ChangePassword(newPassword))).Message.Should().Be(ResAnswers.UserNotFound);
        }
        #endregion
        #region OkObject
        [Fact]
        public async void GetUser_ReturnUser()
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
            UserController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.GetUser();
            var objectResult = result.Result as OkObjectResult;
            var resultUser = objectResult?.Value as User;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(User));
            dbContext.Users.Find(user.Id).Should().Be(user);
            resultUser.Should().Be(user);
        }
        [Fact]
        public async void ChangePassword_ReturnOk()
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
            UserController controller = new(dbContext, _config) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test", Salt = "9f4e9a", PasswordHash = "" };
            string newPassword = "test";
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.ChangePassword(newPassword);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            var newPassHash = AuthTools.HashPasword(newPassword, user.Salt!, _config.Value.AuthData);
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult!.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.Success);
            dbContext.Users.Find(user.Id)!.PasswordHash.Should().Be(newPassHash);
        }
        #endregion
    }
}

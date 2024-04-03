﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.UserRelatedControllers;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Resources;
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
        private readonly IOptions<RouteConfig> _config = new Mock<IOptions<RouteConfig>>().Object;
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
            var okObjectResult = result.Result as OkObjectResult;
            var resultUser = okObjectResult?.Value as User;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(User));
            dbContext.Users.Find(user.Id).Should().Be(user);
            resultUser.Should().Be(user);
        }
        [Fact]
        public async void ChangePassword_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            IOptions<RouteConfig> config = Options.Create<RouteConfig>(new()
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
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("tester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            UserController controller = new(dbContext, config) { ControllerContext = controllerContext };
            User user = new() { Username = "tester", Email = "test", Salt = "9f4e9a", PasswordHash = "" };
            string newPassword = "test";
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.ChangePassword(newPassword);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            var newPassHash = AuthTools.HashPasword(newPassword, user.Salt!, config.Value.AuthData);
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage!.Should().Be(ResAnswers.Success);
            dbContext.Users.Find(user.Id)!.PasswordHash.Should().Be(newPassHash);
        }
        #endregion
    }
}

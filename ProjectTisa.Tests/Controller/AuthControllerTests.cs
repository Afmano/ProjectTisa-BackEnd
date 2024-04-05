using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.UserRelatedControllers;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Requests.UserReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.EF;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using ProjectTisa.Tests.Contexts;

namespace ProjectTisa.Tests.Controller
{
    public class AuthControllerTests
    {
        private readonly ILogger<AuthController> _logger = new Mock<ILogger<AuthController>>().Object;
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
                IssuerSigningKey = "000000000000000000000000000000000",
                IterationCount = 1,
                SaltSize = 1
            },
            CurrentHost = "",
            Version = "",
            ExternalStorage = null,
            SmtpData = new() { Ssl = false, DefaultCredentais = true, FromEmail = "", Password = "", Port = 1, IsTest = true }
        });
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        #region Empty
        [Fact]
        public async void CheckIsEmailExist_Empty_ReturnFalse()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string emailToCheck = "test";
            // Act
            var result = await controller.CheckIsEmailExist(emailToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(false);
        }
        [Fact]
        public async void CheckIsUsernameExist_Empty_ReturnFalse()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string usernameToCheck = "test";
            // Act
            var result = await controller.CheckIsUsernameExist(usernameToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(false);
        }
        [Fact]
        public async void Registrate_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            UserInfoReq userInfo = new() { Email = "test", Password = "password", Username = "test" };
            // Act
            EmailSender.Configure(_config.Value.SmtpData);
            var result = await controller.Registrate(userInfo);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as IdResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(IdResponse));
            resultMessage!.Id.Should().Be(dbContext.PendingRegistrations.Count());
        }
        [Fact]
        public async void Authorize_NoLoginEmail_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            UserLoginReq userLogin = new() { Password = "password" };
            // Act
            var result = await controller.Authorize(userLogin);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void Authorize_NoUser_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            UserLoginReq userLogin = new() { Password = "password", Username = "test" };
            // Act
            var result = await controller.Authorize(userLogin);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void Verify_NoRequest_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            int idToRequest = 1;
            string code = "";
            // Act
            var result = await controller.Verify(idToRequest, code);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        #endregion
        #region Filled
        #region OkResult
        [Fact]
        public async void Authorize_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string password = "password";
            string salt = "9f4e9a";
            string username = "test";
            User user = new() { Email = "", Username = username, PasswordHash = AuthTools.HashPasword(password, salt, _config.Value.AuthData), Salt = salt };
            UserLoginReq userLogin = new() { Password = password, Username = username };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.Authorize(userLogin);
            var objectResult = result.Result as OkObjectResult;
            var tokenResult = objectResult?.Value as TokenResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(TokenResponse));
            tokenResult!.Token.Should().NotBeNull();
        }
        [Fact]
        public async void Verify_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string code = "1";
            string username = "test";
            PendingRegistration pendingRegistration = new() { Email = "", ExpireDate = DateTime.UtcNow.AddDays(1), PasswordHash = "", Salt = "", Username = username, VerificationCode = code };
            // Act
            dbContext.Add(pendingRegistration);
            await dbContext.SaveChangesAsync();
            var result = await controller.Verify(pendingRegistration.Id, code);
            var objectResult = result.Result as OkObjectResult;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(TokenResponse));
            dbContext.Users.First().Username.Should().Be(username);
        }
        #endregion
        #region Bool
        [Fact]
        public async void CheckIsEmailExist_Filled_ReturnFalse()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string emailToCheck = "test";
            User user = new() { Email = "", Username = "" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.CheckIsEmailExist(emailToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(false);
        }
        [Fact]
        public async void CheckIsUsernameExist_Filled_ReturnFalse()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string usernameToCheck = "test";
            User user = new() { Email = "", Username = "" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.CheckIsUsernameExist(usernameToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(false);
        }
        [Fact]
        public async void CheckIsEmailExist_Filled_ReturnTrue()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string emailToCheck = "test";
            User user = new() { Email = emailToCheck, Username = "" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.CheckIsEmailExist(emailToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(true);
        }
        [Fact]
        public async void CheckIsUsernameExist_Filled_ReturnTrue()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string usernameToCheck = "test";
            User user = new() { Email = "", Username = usernameToCheck };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.CheckIsUsernameExist(usernameToCheck);
            var objectResult = result.Result as OkObjectResult;
            var resultMessage = objectResult?.Value as BooleanResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(BooleanResponse));
            resultMessage!.Result.Should().Be(true);
        }
        [Fact]
        #endregion Bool
        #region BadRequest
        public async void Registrate_EmailExist_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string emailToRegistrate = "test";
            UserInfoReq userInfo = new() { Email = emailToRegistrate, Password = "password", Username = "tester" };
            User user = new() { Email = emailToRegistrate, Username = "" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            EmailSender.Configure(_config.Value.SmtpData);
            var result = await controller.Registrate(userInfo);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.EmailUsernameExist);
        }
        [Fact]
        public async void Registrate_UsernameExist_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string usernameToRegistrate = "test";
            UserInfoReq userInfo = new() { Email = "test", Password = "password", Username = usernameToRegistrate };
            User user = new() { Email = "", Username = usernameToRegistrate };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            EmailSender.Configure(_config.Value.SmtpData);
            var result = await controller.Registrate(userInfo);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.EmailUsernameExist);
        }
        [Fact]
        public async void Verify_WrongCode_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            PendingRegistration pendingRegistration = new() { Email = "", ExpireDate = DateTime.UtcNow.AddDays(1), PasswordHash = "", Salt = "", Username = "", VerificationCode = "1111" };
            // Act
            dbContext.Add(pendingRegistration);
            await dbContext.SaveChangesAsync();
            var result = await controller.Verify(pendingRegistration.Id, "2");
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void Verify_Expired_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string code = "1";
            PendingRegistration pendingRegistration = new() { Email = "", ExpireDate = DateTime.UtcNow.AddDays(-1), PasswordHash = "", Salt = "", Username = "", VerificationCode = code };
            // Act
            dbContext.Add(pendingRegistration);
            await dbContext.SaveChangesAsync();
            var result = await controller.Verify(pendingRegistration.Id, code);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void Authorize_WrongPass_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            AuthController controller = new(_logger, dbContext, _config);
            string username = "test";
            UserLoginReq userLogin = new() { Password = "wrongPassword", Username = username };
            User user = new() { Email = "", Username = username, PasswordHash = "0000", Salt = "9f4e9a" };
            // Act
            dbContext.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.Authorize(userLogin);
            var objectResult = result.Result as BadRequestObjectResult;
            var resultMessage = objectResult?.Value as MessageResponse;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            objectResult?.Value.Should().NotBeNull();
            objectResult!.Value.Should().BeOfType(typeof(MessageResponse));
            resultMessage!.Message.Should().Be(ResAnswers.BadRequest);
        }
        #endregion
        #endregion
    }
}

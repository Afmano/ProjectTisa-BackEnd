﻿using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using ProjectTisa.Controllers.BusinessControllers.CrudControllers;
using ProjectTisa.Controllers.GeneralData.Requests;
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
    public class OrderControllerTests
    {
        private readonly ILogger<OrderController> _logger = new Mock<ILogger<OrderController>>().Object;
        private readonly IAuthorizationService _authorizationService = new Mock<IAuthorizationService>().Object;
        #region empty
        [Fact]
        public async void GetAll_ReturnEmptyList()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            // Act
            var result = await controller.Get(paginationRequest);
            var okObjectResult = result.Result as OkObjectResult;
            var orders = okObjectResult?.Value as List<Order>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Order>));
            orders!.Count.Should().Be(0);
        }
        [Fact]
        public async void GetById_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
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
        public async void Update_WrongOrderId_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            int idToRequest = 1;
            // Act
            dbContext.Products.Add(product);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(idToRequest, new() { UserId = user.Id, ProductIdQuantities = [new() { ProductId = product.Id, Quantity = 10 }] });//validation attributes ignored here
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
        public async void CompleteOrder_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
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
        public async void CancelOrder_ReturnNotFound()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            int idToRequest = 1;
            // Act
            var result = await controller.CancelOrder(idToRequest);
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
        #region filled
        #region OkResult
        [Fact]
        public async void GetAll_ByStatusInProgress_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order orderInProgress = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCompleted = new() { EditInfo = new("tester"), Status = OrderStatus.Completed, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCancelled = new() { EditInfo = new("tester"), Status = OrderStatus.Cancelled, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            OrderStatus orderStatusToSearch = OrderStatus.InProgress;
            // Act
            dbContext.Orders.AddRange(orderInProgress, orderCompleted, orderCancelled);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest, orderStatusToSearch);
            var okObjectResult = result.Result as OkObjectResult;
            var orders = okObjectResult?.Value as List<Order>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Order>));
            orders!.Count.Should().NotBe(0);
            orders!.First().Status.Should().Be(orderStatusToSearch);
        }
        [Fact]
        public async void GetAll_ByStatusCompleted_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order orderInProgress = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCompleted = new() { EditInfo = new("tester"), Status = OrderStatus.Completed, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCancelled = new() { EditInfo = new("tester"), Status = OrderStatus.Cancelled, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            OrderStatus orderStatusToSearch = OrderStatus.Completed;
            // Act
            dbContext.Orders.AddRange(orderInProgress, orderCompleted, orderCancelled);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest, orderStatusToSearch);
            var okObjectResult = result.Result as OkObjectResult;
            var orders = okObjectResult?.Value as List<Order>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Order>));
            orders!.Count.Should().NotBe(0);
            orders!.First().Status.Should().Be(orderStatusToSearch);
        }
        [Fact]
        public async void GetAll_ByStatusCancelled_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            PaginationRequest paginationRequest = new();
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order orderInProgress = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCompleted = new() { EditInfo = new("tester"), Status = OrderStatus.Completed, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            Order orderCancelled = new() { EditInfo = new("tester"), Status = OrderStatus.Cancelled, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            OrderStatus orderStatusToSearch = OrderStatus.Cancelled;
            // Act
            dbContext.Orders.AddRange(orderInProgress, orderCompleted, orderCancelled);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(paginationRequest, orderStatusToSearch);
            var okObjectResult = result.Result as OkObjectResult;
            var orders = okObjectResult?.Value as List<Order>;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult?.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(List<Order>));
            orders!.Count.Should().NotBe(0);
            orders!.First().Status.Should().Be(orderStatusToSearch);
        }
        [Fact]
        public async void GetById_ReturnOk()
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(order.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var resultOrder = okObjectResult?.Value as Order;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(Order));
            resultOrder!.TotalPrice.Should().Be(order.TotalPrice);
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "email", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            uint quantity = 5;
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(order.Id, new() { UserId = user.Id, ProductIdQuantities = [new() { ProductId = product.Id, Quantity = quantity }] });//validation attributes ignored here
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            dbContext.Orders.Find(order.Id)!.ProductQuantities.Sum(x => x.Product.Price * x.Quantity).Should().Be(product.Price * quantity);
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            User user = new() { Email = "", Username = "tester" };
            uint quantity = 5;
            // Act
            dbContext.Users.Add(user);
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Create(new() { UserId = user.Id, ProductIdQuantities = [new() { ProductId = product.Id, Quantity = quantity }] });//validation attributes ignored here
            var createdResult = result.Result as CreatedResult;
            var resultMessage = createdResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(CreatedResult));
            createdResult!.Value.Should().NotBeNull();
            createdResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Created);
            dbContext.Orders.Count().Should().Be(1);
            dbContext.Orders.FirstOrDefault()!.ProductQuantities.Sum(x => x.Product.Price * x.Quantity).Should().Be(product.Price * quantity);
        }
        [Fact]
        public async void CompleteOrder_ReturnOk()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.CompleteOrder(order.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            order.Status.Should().Be(OrderStatus.Completed);
        }
        [Fact]
        public async void CancelOrder_ReturnOk()
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.CancelOrder(order.Id);
            var okObjectResult = result.Result as OkObjectResult;
            var resultMessage = okObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            okObjectResult!.Value.Should().NotBeNull();
            okObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.Success);
            order.Status.Should().Be(OrderStatus.Cancelled);
        }
        #endregion
        #region BadRequest
        [Fact]
        public async void CompleteOrder_ReturnBadRequest()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            OrderController controller = new(_logger, dbContext, _authorizationService);
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.Completed, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.CompleteOrder(order.Id);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult!.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void CancelOrder_ReturnBadRequest()
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.Cancelled, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.CancelOrder(order.Id);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult!.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
        }
        [Fact]
        public async void Create_ReturnBadRequest()
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            // Act
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var result = await controller.Create(new() { UserId = user.Id, ProductIdQuantities = [] });//validation attributes ignored here
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult!.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
            dbContext.Orders.Count().Should().Be(0);
        }
        [Fact]
        public async void Update_ReturnBadRequest()
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
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.Update(order.Id, new() { UserId = order.User.Id, ProductIdQuantities = [] });//validation attributes ignored here
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            var resultMessage = badRequestObjectResult?.Value as string;
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(BadRequestObjectResult));
            badRequestObjectResult!.Value.Should().NotBeNull();
            badRequestObjectResult!.Value.Should().BeOfType(typeof(string));
            resultMessage.Should().Be(ResAnswers.BadRequest);
        }
        #endregion
        #region Forbid
        [Fact]
        public async void GetById_ReturnForbid()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("wrongTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            var customMockAuthorizeService = new Mock<IAuthorizationService>();
            customMockAuthorizeService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>())).ReturnsAsync(AuthorizationResult.Failed);
            OrderController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.Get(order.Id);
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(ForbidResult));
        }
        [Fact]
        public async void Create_ReturnForbid()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("wrongTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            OrderController controller = new(_logger, dbContext, _authorizationService) { ControllerContext = controllerContext };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            User user = new() { Email = "", Username = "tester" };
            // Act
            dbContext.Users.Add(user);
            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync();
            var result = await controller.Create(new() { UserId = user.Id, ProductIdQuantities = [new() { ProductId = product.Id, Quantity = 5 }] });//validation attributes ignored here
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(ForbidResult));
        }
        [Fact]
        public async void CancelOrder_ReturnForbid()
        {
            // Arrange
            MainDbContext dbContext = DatabaseContext.SetUpContext();
            DefaultHttpContext httpContext = new()
            {
                User = new ClaimsPrincipal(new GenericIdentity("wrongTester", "test"))
            };
            ControllerContext controllerContext = new()
            {
                HttpContext = httpContext,
            };
            var customMockAuthorizeService = new Mock<IAuthorizationService>();
            customMockAuthorizeService.Setup(service => service.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<string>())).ReturnsAsync(AuthorizationResult.Failed);
            OrderController controller = new(_logger, dbContext, customMockAuthorizeService.Object) { ControllerContext = controllerContext };
            User user = new() { Email = "", Username = "tester" };
            Category category = new() { Name = "", PhotoPath = "", EditInfo = new("tester") };
            Product product = new() { Name = "", PhotoPath = "", EditInfo = new("tester"), Category = category, IsAvailable = true, Price = 1 };
            Order order = new() { EditInfo = new("tester"), Status = OrderStatus.InProgress, TotalPrice = 1, User = user, ProductQuantities = [new() { Product = product, Quantity = 1 }] };
            // Act
            dbContext.Orders.Add(order);
            await dbContext.SaveChangesAsync();
            var result = await controller.CancelOrder(order.Id);
            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(ForbidResult));
        }
        #endregion
        #endregion
    }
}

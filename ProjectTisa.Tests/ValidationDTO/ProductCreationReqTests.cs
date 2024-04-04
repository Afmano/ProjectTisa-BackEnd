using FluentAssertions;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Tests.ValidationDTO
{
    public class ProductCreationReqTests
    {
        [Theory]
        [InlineData("", "", -1d, "")]
        [InlineData("1", "1", -100d, "1")]
        [InlineData("lolll!", "l", -0.1d, "l")]
        public void WrongAll_ReturnFalse(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Name)) || res.ErrorMessage!.Contains(nameof(toValidate.PhotoPath))|| res.ErrorMessage!.Contains(nameof(toValidate.Price))|| res.ErrorMessage!.Contains(nameof(toValidate.Tags))).Should().BeTrue();
        }
        [Theory]
        [InlineData("", "https://www.google.com/", 0d, "test")]
        [InlineData("1", "http://www.test.com", 0.5d, "test2")]
        [InlineData("lolll!", "ftp://www.google.com", 1d, "test3")]
        public void WrongName_ReturnFalse(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Name))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test1", "", 0d, "test")]
        [InlineData("test2", "1", 0.5d, "test2")]
        [InlineData("test3", "l", 1d, "test3")]
        public void WrongPath_ReturnFalse(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.PhotoPath))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test1", "https://www.google.com/", -1d, "test")]
        [InlineData("test2", "http://www.test.com", -100d, "test2")]
        [InlineData("test3", "ftp://www.google.com", -0.1d, "test3")]
        public void WrongPrice_ReturnFalse(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Price))).Should().BeTrue();
        }

        [Theory]
        [InlineData("test1", "https://www.google.com/", 0d, "")]
        [InlineData("test2", "http://www.test.com", 0.5d, "l")]
        [InlineData("test3", "ftp://www.google.com", 1d, "test3!")]
        public void WrongTag_ReturnFalse(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Tags))).Should().BeTrue();
        }

        [Theory]
        [InlineData("test1", "https://www.google.com/", 0d, "test")]
        [InlineData("test2", "http://www.test.com", 0.5d, "test2")]
        [InlineData("test3", "ftp://www.google.com", 1d, "test3")]
        public void ReturnTrue(string name, string photoPath, decimal price, string tag)
        {
            // Arrange
            ProductCreationReq toValidate = new() { Name = name, PhotoPath = photoPath, Price = price, CategoryId = 0, IsAvailable = false, Tags = [tag] };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeTrue();
            valResults.Capacity.Should().Be(0);
        }
    }
}

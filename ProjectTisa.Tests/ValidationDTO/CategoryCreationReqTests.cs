using FluentAssertions;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Tests.ValidationDTO
{
    public class CategoryCreationReqTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("1", "111")]
        [InlineData("lolll!", "lol")]
        public void WrongAll_ReturnFalse(string name, string photoPath)
        {
            // Arrange
            CategoryCreationReq toValidate = new() { Name = name, PhotoPath = photoPath };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Name)) || res.ErrorMessage!.Contains(nameof(toValidate.PhotoPath))).Should().BeTrue();
        }
        [Theory]
        [InlineData("tester1", "")]
        [InlineData("tester2", "111")]
        [InlineData("tester3", "lol")]
        public void WrongPath_ReturnFalse(string name, string photoPath)
        {
            // Arrange
            CategoryCreationReq toValidate = new() { Name = name, PhotoPath = photoPath };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Capacity.Should().NotBe(0);
            valResults.All(res => !res.ErrorMessage!.Contains(nameof(toValidate.Name))).Should().BeTrue();
        }
        [Theory]
        [InlineData("", "https://www.google.com/")]
        [InlineData("1", "http://www.test.com")]
        [InlineData("lolll!", "ftp://www.google.com")]
        public void WrongName_ReturnFalse(string name, string photoPath)
        {
            // Arrange
            CategoryCreationReq toValidate = new() { Name = name, PhotoPath = photoPath };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Capacity.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Name))).Should().BeTrue();
        }
        [Theory]
        [InlineData("tester1", "https://www.google.com/")]
        [InlineData("tester2", "http://www.test.com")]
        [InlineData("tester3", "ftp://www.google.com")]
        public void ReturnTrue(string name, string photoPath)
        {
            // Arrange
            CategoryCreationReq toValidate = new() { Name = name, PhotoPath = photoPath };
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

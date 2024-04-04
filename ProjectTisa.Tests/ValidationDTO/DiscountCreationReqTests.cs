using FluentAssertions;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Tests.ValidationDTO
{
    public class DiscountCreationReqTests
    {
        [Theory]
        [InlineData("", -1d)]
        [InlineData("1", 2d)]
        [InlineData("lolll!", 100d)]
        public void WrongAll_ReturnFalse(string name, decimal percent)
        {
            // Arrange
            DiscountCreationReq toValidate = new() { Name = name, DiscountPercent = percent };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Name)) || res.ErrorMessage!.Contains(nameof(toValidate.DiscountPercent))).Should().BeTrue();
        }
        [Theory]
        [InlineData("tester1", -1)]
        [InlineData("tester2", 2d)]
        [InlineData("tester3", 100d)]
        public void WrongPercent_ReturnFalse(string name, decimal percent)
        {
            // Arrange
            DiscountCreationReq toValidate = new() { Name = name, DiscountPercent = percent };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Capacity.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.DiscountPercent))).Should().BeTrue();
        }
        [Theory]
        [InlineData("", 0d)]
        [InlineData("1", 0.5d)]
        [InlineData("lolll!", 1d)]
        public void WrongName_ReturnFalse(string name, decimal percent)
        {
            // Arrange
            DiscountCreationReq toValidate = new() { Name = name, DiscountPercent = percent };
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
        [InlineData("tester1", 0d)]
        [InlineData("tester2", 0.5d)]
        [InlineData("tester3", 1d)]
        public void ReturnTrue(string name, decimal percent)
        {
            // Arrange
            DiscountCreationReq toValidate = new() { Name = name, DiscountPercent = percent };
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

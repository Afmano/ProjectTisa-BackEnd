using FluentAssertions;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Tests.ValidationDTO
{
    public class NotificationCreationReqTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("1")]
        [InlineData("lolll!")]
        public void ReturnFalse(string caption)
        {
            // Arrange
            NotificationCreationReq toValidate = new() { Caption = caption, Message = "" };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Caption))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        [InlineData("test3")]
        public void ReturnTrue(string caption)
        {
            // Arrange
            NotificationCreationReq toValidate = new() { Caption = caption, Message = "" };
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

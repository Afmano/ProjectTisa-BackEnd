using FluentAssertions;
using ProjectTisa.Controllers.GeneralData.Requests.UserReq;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Tests.ValidationDTO
{
    public class UserInfoReqTests
    {
        [Theory]
        [InlineData("", "", "")]
        [InlineData("l", "l", "loool")]
        [InlineData("lolll!", "111222333!", "lolll!")]
        public void WrongAll_ReturnFalse(string caption, string password, string email)
        {
            // Arrange
            UserInfoReq toValidate = new() { Username = caption, Password = password, Email = email };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Username)) || res.ErrorMessage!.Contains(nameof(toValidate.Password)) || res.ErrorMessage!.Contains(nameof(toValidate.Email))).Should().BeTrue();
        }
        [Theory]
        [InlineData("", "3zjqExB2", "test@dasf.tt")]
        [InlineData("l", "Cp04PDC0yvK", "test@dasf.com")]
        [InlineData("lolll!", "EribiZ7b0SHhzFat", "test111122@gmail.com")]
        public void WrongUsername_ReturnFalse(string caption, string password, string email)
        {
            // Arrange
            UserInfoReq toValidate = new() { Username = caption, Password = password, Email = email };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Username))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test", "111222333", "test@dasf.tt")]
        [InlineData("test2", "222233444", "test@dasf.com")]
        [InlineData("testtt3", "1111344422", "test111122@gmail.com")]
        public void WrongPassword_ReturnFalse(string caption, string password, string email)
        {
            // Arrange
            UserInfoReq toValidate = new() { Username = caption, Password = password, Email = email };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Password))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test", "3zjqExB2", "")]
        [InlineData("test2", "Cp04PDC0yvK", "test")]
        [InlineData("testtt3", "EribiZ7b0SHhzFat", "test!@!@#111122@gmail.com")]
        public void WrongEmail_ReturnFalse(string caption, string password, string email)
        {
            // Arrange
            UserInfoReq toValidate = new() { Username = caption, Password = password, Email = email };
            ValidationContext valContext = new(toValidate);
            List<ValidationResult> valResults = [];
            // Act
            var validationResult = Validator.TryValidateObject(toValidate, valContext, valResults, true);
            // Assert
            validationResult.Should().BeFalse();
            valResults.Count.Should().NotBe(0);
            valResults.All(res => res.ErrorMessage!.Contains(nameof(toValidate.Email))).Should().BeTrue();
        }
        [Theory]
        [InlineData("test", "3zjqExB2", "test@dasf.tt")]
        [InlineData("test2", "Cp04PDC0yvK", "test@dasf.com")]
        [InlineData("testtt3", "EribiZ7b0SHhzFat", "test111122@gmail.com")]
        public void ReturnTrue(string caption, string password, string email)
        {
            // Arrange
            UserInfoReq toValidate = new() { Username = caption, Password = password, Email = email };
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

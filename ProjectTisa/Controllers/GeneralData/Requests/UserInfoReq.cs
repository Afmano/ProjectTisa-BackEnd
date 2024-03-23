using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests
{
    /// <summary>
    /// Request for creating user. Contains <c>Username</c>, <c>Password</c> and <c>Email</c>.
    /// </summary>
    public record class UserInfoReq
    {
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Username { get; set; }
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Password { get; set; }
        [EmailAddress]
        [RegularExpression(ValidationConst.REGEX_EMAIL)]
        [StringLength(ValidationConst.MAX_EMAIL_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Email { get; set; }
        public static UserInfoReq GetChangePassword(User user, string password)
        {
            return new UserInfoReq() { Email = user.Email, Username = user.Username, Password = password };
        }
        public static List<ValidationResult> Validate(UserInfoReq userInfo)
        {
            ValidationContext valContext = new(userInfo);
            List<ValidationResult> valResults = [];
            Validator.TryValidateObject(userInfo, valContext, valResults, true);
            return valResults;
        }
    }
}

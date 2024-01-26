using ProjectTisa.Controllers.GeneralData.Consts;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests
{
    /// <summary>
    /// Request for creating user. Contains <c>Username</c>, <c>Password</c> and <c>Email</c>.
    /// </summary>
    public record class UserCreationReq
    {
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Username { get; set; }
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Password { get; set; }
        [EmailAddress]
        [RegularExpression(ValidationConst.REGEX_NUM_SYMBS)]
        [StringLength(ValidationConst.MAX_STR_LENGTH, MinimumLength = ValidationConst.MIN_STR_LENGTH)]
        public required string Email { get; set; }
    }
}

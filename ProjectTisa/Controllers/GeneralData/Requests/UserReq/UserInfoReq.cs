﻿using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Validation;
using ProjectTisa.Controllers.GeneralData.Validation.Enums;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.GeneralData.Requests.UserReq
{
    /// <summary>
    /// Request for creating user. Contains <c>Username</c>, <c>Password</c> and <c>Email</c>.
    /// </summary>
    public record class UserInfoReq
    {
        [StringRequirements(StringMaxLengthType.Username)]
        public required string Username { get; set; }
        [StringRequirements]
        public required string Password { get; set; }
        [EmailAddress]
        [StringRequirements(StringMaxLengthType.Domain, ValidationConst.REGEX_EMAIL)]
        public required string Email { get; set; }
        public static UserInfoReq GetChangePassword(User user, string password)
        {
            return new UserInfoReq() { Email = user.Email, Username = user.Username, Password = password };
        }
    }
}

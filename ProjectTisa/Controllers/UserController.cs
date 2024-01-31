using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Exceptions;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(MainDbContext context, IOptions<RouteConfig> config) : ControllerBase
    {
        /// <summary>
        /// Get <see cref="User"/>'s info from JWT token in Authorization header.
        /// </summary>
        /// <returns>200: JSON of <see cref="User"/>.</returns>
        [Authorize]
        [HttpGet("GetUser")]
        public ActionResult<string> GetUser()
        {
            return Ok(GetUserFromContext());
        }
        /// <summary>
        /// Change <see cref="User"/>'s password using same salt.
        /// </summary>
        /// <param name="password">Password in string format.</param>
        /// <returns>200: message.</returns>
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword([FromBody] string password)
        {
            User user = GetUserFromContext();
            List<ValidationResult> valResults = UserInfoReq.Validate(UserInfoReq.GetChangePassword(user, password));
            if (valResults.Count > 0)
            {
                return BadRequest(valResults);
            }
            user.PasswordHash = AuthTools.HashPasword(password, user.Salt!, config.Value.AuthData);
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
        [HttpPost("ChangeEmail")]
        public ActionResult ChangeEmail([FromBody] string email)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Reading JWT token from Authorization header return <see cref="User"/> from database.
        /// </summary>
        /// <returns><see cref="User"/> class.</returns>
        /// <exception cref="ControllerException">If JWT is wrong or user not found.</exception>
        private User GetUserFromContext()
        {
            User user;
            string currentUsername = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value ?? throw new ControllerException(ResAnswers.WrongJWT);
            user = context.Users.FirstOrDefault(x => x.Username.Equals(currentUsername)) ?? throw new ControllerException(ResAnswers.UserNorFound);
            user.LastSeen = DateTime.UtcNow;
            return user;
        }
    }
}

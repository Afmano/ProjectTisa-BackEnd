using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Requests.UserReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.UserRelatedControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(MainDbContext context, IOptions<RouteConfig> config) : ControllerBase
    {
        /// <summary>
        /// Get <see cref="User"/>'s info from JWT token in Authorization header.
        /// </summary>
        /// <returns>200: JSON of <see cref="User"/>.</returns>
        [HttpGet("GetUser")]
        public ActionResult<User> GetUser()
        {
            return Ok(GetCurrentUser());
        }
        /// <summary>
        /// Change <see cref="User"/>'s password using same salt.
        /// </summary>
        /// <param name="password">Password in string format.</param>
        /// <returns>200: message.</returns>
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword([FromBody] string password)
        {
            User user = await GetCurrentUser();
            List<ValidationResult> valResults = ObjectsUtils.Validate(UserInfoReq.GetChangePassword(user, password));
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
        private async Task<User> GetCurrentUser() => await ObjectsUtils.GetUserFromContext(HttpContext, context);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.Controllers.GeneralData.Validation.Attributes;
using ProjectTisa.Libs;
using ProjectTisa.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectTisa.Controllers.BusinessControllers.UserRelatedControllers
{
    /// <summary>
    /// Controller to interact with <see cref="User"/>. <b>Required <see cref="AuthorizeAttribute"/> authorization</b>.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="config"></param>
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
        [ProducesResponseType(200, Type = typeof(User))]
        public async Task<ActionResult<User>> GetUser() => Ok(await GetCurrentUser());
        /// <summary>
        /// Change <see cref="User"/>'s password using same salt.
        /// </summary>
        /// <param name="password">Password in string format.</param>
        /// <returns>200: message.</returns>
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<MessageResponse>> ChangePassword([StringRequirements(regularExpression: ValidationConst.REGEX_PASSWORD)][FromBody] string password)
        {
            User user = await GetCurrentUser();
            user.PasswordHash = AuthTools.HashPasword(password, user.Salt!, config.Value.AuthData);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        /// <summary>
        /// Currently in development.
        /// </summary>
        /// <param name="email">New email to change.</param>
        /// <returns>200: message.</returns>
        [HttpPost("ChangeEmail")]
        public ActionResult ChangeEmail([EmailAddress][StringRequirements][FromBody] string email)
        {
            throw new NotImplementedException();//add later
        }
        private async Task<User> GetCurrentUser() => await UserUtils.GetUserFromContext(HttpContext, context);
    }
}

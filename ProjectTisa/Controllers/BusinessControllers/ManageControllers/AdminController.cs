using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;
using ProjectTisa.Models.Enums;

namespace ProjectTisa.Controllers.BusinessControllers.RoleControllers
{
    /// <summary>
    /// Controller with methods for Admin role. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>admin</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "admin")]
    public class AdminController(MainDbContext context) : ControllerBase
    {
        /// <summary>
        /// Set <see cref="RoleType"/> to <see cref="User"/>. Can't interact with <c>Admin</c>+ roles.
        /// </summary>
        /// <param name="userId">Id of user to set role.</param>
        /// <param name="role">Role to set.</param>
        /// <returns>200: message.</returns>
        [HttpPatch("{userId}")]
        public async Task<ActionResult<string>> SetRoleToUser(int userId, [FromBody] RoleType role)
        {
            User? user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            if(user.Role >= RoleType.Admin || role >= RoleType.Admin || user.Username == User.Identity!.Name)
            {
                return BadRequest(ResAnswers.BadRequest);
            }

            user.Role = role;
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
    }
}

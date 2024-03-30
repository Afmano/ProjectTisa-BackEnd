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

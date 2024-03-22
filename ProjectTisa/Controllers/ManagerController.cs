using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;
using ProjectTisa.Models.Enums;

namespace ProjectTisa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager")]
    public class ManagerController(MainDbContext context) : ControllerBase
    {
        /// <summary>
        /// Send <see cref="Notification"/> for all users by default, or by parameters.
        /// </summary>
        /// <returns>200: message.</returns>
        [HttpPost("SendNotificationByRole")]
        public async Task<ActionResult<string>> SendNotificationByRole(int id, RoleType userRole)
        {
            Notification? notif = await context.Notifications.FindAsync(id);
            if (notif == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            var test = context.Users.Where(user => user.Role == userRole).ToList();
            test.ForEach(user => user.Notifications.Add(notif));
            await context.SaveChangesAsync();

            return Ok(ResAnswers.Success);
        }
    }
}

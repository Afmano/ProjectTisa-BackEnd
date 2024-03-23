using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;
using ProjectTisa.Models.Enums;

namespace ProjectTisa.Controllers
{
    /// <summary>
    /// Controller with methods for Manager role. <b>Required <see cref="AuthorizeAttribute"/> role:</b> <c>Admin</c> or <c>Manager</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager")]
    public class ManagerController(MainDbContext context) : ControllerBase
    {
        /// <summary>
        /// Send <see cref="Notification"/> for users by role.
        /// </summary>
        /// <param name="userRole">Role of users for notificate.</param>
        /// <param name="notification">Notification to send.</param>
        /// <returns>200: message.</returns>
        [HttpPost("SendNotificationByRole")]
        public async Task<ActionResult<string>> SendNotificationByRole(RoleType userRole, [FromBody] Notification notification)
        {
            List<User> usersToNotificate = [.. context.Users.Where(user => user.Role == userRole)];
            if (usersToNotificate.Count == 0)
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.AddRange(usersToNotificate);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            return Ok(ResAnswers.Success);
        }
        /// <summary>
        /// Send <see cref="Notification"/> for user by username.
        /// </summary>
        /// <param name="username">Username of user for notificate.</param>
        /// <param name="notification">Notification to send.</param>
        /// <returns>200: message.</returns>
        [HttpPost("SendNotificationByUsername")]
        public async Task<ActionResult<string>> SendNotificationByUsername(string username, [FromBody] Notification notification)
        {
            User userToNotificate = context.Users.First(user => user.Username == username);
            if (userToNotificate == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.Add(userToNotificate);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            return Ok(ResAnswers.Success);
        }
        /// <summary>
        /// Send <see cref="Notification"/> for user by email.
        /// </summary>
        /// <param name="email">Email of user for notificate.</param>
        /// <param name="notification">Notification to send.</param>
        /// <returns>200: message.</returns>
        [HttpPost("SendNotificationByEmail")]
        public async Task<ActionResult<string>> SendNotificationByEmail(string email, [FromBody] Notification notification)
        {
            User userToNotificate = context.Users.First(user => user.Email == email);
            if (userToNotificate == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.Add(userToNotificate);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            return Ok(ResAnswers.Success);
        }
    }
}

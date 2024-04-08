using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.Models;
using ProjectTisa.Models.BusinessLogic;
using ProjectTisa.Models.Enums;
using System.Net.Http.Headers;

namespace ProjectTisa.Controllers.BusinessControllers.RoleControllers
{
    /// <summary>
    /// Controller with specific methods for Manager role. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c>.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "manage")]
    public class ManagerController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly MainDbContext _mainDbContext;
        private readonly ExternalStorage _externalStorage;
        public ManagerController(MainDbContext context, IOptions<RouteConfig> config)
        {
            _client = new();
            _mainDbContext = context;
            _externalStorage = config.Value.ExternalStorage;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _externalStorage.Auth);
        }
        #region Notification
        /// <summary>
        /// Send <see cref="Notification"/> for users by role.
        /// </summary>
        /// <param name="notificationId">Id of notification to send.</param>
        /// <param name="userRole">Role of users for notificate.</param>
        /// <returns>200: message.</returns>
        [HttpPatch("SendNotificationByRole")]
        public async Task<ActionResult<MessageResponse>> SendNotificationByRole(int notificationId, RoleType userRole)
        {
            Notification? notification = await _mainDbContext.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            List<User> usersToNotificate = [.. _mainDbContext.Users.Where(user => user.Role == userRole)];
            if (usersToNotificate.Count == 0)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullContext));
            }

            notification.EditInfo.Modify(User.Identity!.Name!);
            notification.Users.AddRange(usersToNotificate);
            await _mainDbContext.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        /// <summary>
        /// Send <see cref="Notification"/> for user by username.
        /// </summary>
        /// <param name="notificationId">Id of notification to send.</param>
        /// <param name="username">Username of user for notificate.</param>
        /// <returns>200: message.</returns>
        [HttpPatch("SendNotificationByUsername")]
        public async Task<ActionResult<MessageResponse>> SendNotificationByUsername(int notificationId, string username)
        {
            Notification? notification = await _mainDbContext.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            User? userToNotificate = await _mainDbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
            if (userToNotificate == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            notification.EditInfo.Modify(User.Identity!.Name!);
            notification.Users.Add(userToNotificate);
            await _mainDbContext.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        /// <summary>
        /// Send <see cref="Notification"/> for user by email.
        /// </summary>
        /// <param name="notificationId">Id of notification to send.</param>
        /// <param name="email">Email of user for notificate.</param>
        /// <returns>200: message.</returns>
        [HttpPatch("SendNotificationByEmail")]
        public async Task<ActionResult<MessageResponse>> SendNotificationByEmail(int notificationId, string email)
        {
            Notification? notification = await _mainDbContext.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            User? userToNotificate = await _mainDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (userToNotificate == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            notification.EditInfo.Modify(User.Identity!.Name!);
            notification.Users.Add(userToNotificate);
            await _mainDbContext.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        /// <summary>
        /// Detach all users from selected <see cref="Notification"/>>.
        /// </summary>
        /// <param name="notificationId">Id of notification to edit.</param>
        /// <returns>200: message.</returns>
        [HttpPatch("DetachNotification")]
        public async Task<ActionResult<MessageResponse>> DetachNotification(int notificationId)
        {
            Notification? notification = await _mainDbContext.Notifications.FindAsync(notificationId);
            if (notification == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            notification.EditInfo.Modify(User.Identity!.Name!);
            notification.Users.Clear();
            await _mainDbContext.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        #endregion
        #region File
        /// <summary>
        /// Load file to server external storage.
        /// </summary>
        /// <returns>200: <c>IpfsHash</c> to resource.</returns>
        [HttpPost("LoadFile")]
        [Produces("application/json")]
        [RequestFormLimits(MultipartBodyLengthLimit = ValidationConst.MAX_FILE_SIZE)]
        public async Task<ActionResult<string>> LoadFile(IFormFile file)
        {
            MultipartFormDataContent content = new()
            {
                { new StreamContent(file.OpenReadStream()), file.Name, file.FileName }
            };
            HttpResponseMessage response = await _client.PostAsync(_externalStorage.PostUrl, content);
            return StatusCode((int)response.StatusCode, new { response = await response.Content.ReadAsStringAsync() });
        }
        /// <summary>
        /// Get list of files from external storage.
        /// </summary>
        /// <returns>200: json with files data.</returns>
        [HttpGet("GetFiles")]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetFiles()
        {
            HttpResponseMessage response = await _client.GetAsync(_externalStorage.GetUrl);
            return StatusCode((int)response.StatusCode, new { response = await response.Content.ReadAsStringAsync() });
        }
        #endregion
    }
}

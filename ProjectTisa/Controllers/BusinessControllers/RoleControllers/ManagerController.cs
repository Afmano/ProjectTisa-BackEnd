﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProjectTisa.Controllers.GeneralData.Configs;
using ProjectTisa.Controllers.GeneralData.Consts;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Models;
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
        /// <param name="userRole">Role of users for notificate.</param>
        /// <param name="notification">Notification to send.</param>
        /// <returns>200: message.</returns>
        [HttpPost("SendNotificationByRole")]
        public async Task<ActionResult<string>> SendNotificationByRole(RoleType userRole, [FromBody] Notification notification)
        {
            List<User> usersToNotificate = [.. _mainDbContext.Users.Where(user => user.Role == userRole)];
            if (usersToNotificate.Count == 0)
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.AddRange(usersToNotificate);
            _mainDbContext.Notifications.Add(notification);
            await _mainDbContext.SaveChangesAsync();

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
            User userToNotificate = _mainDbContext.Users.First(user => user.Username == username);
            if (userToNotificate == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.Add(userToNotificate);
            _mainDbContext.Notifications.Add(notification);
            await _mainDbContext.SaveChangesAsync();

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
            User userToNotificate = _mainDbContext.Users.First(user => user.Email == email);
            if (userToNotificate == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }
            notification.CreationTime = DateTime.UtcNow;
            notification.Users.Add(userToNotificate);
            _mainDbContext.Notifications.Add(notification);
            await _mainDbContext.SaveChangesAsync();

            return Ok(ResAnswers.Success);
        }
        #endregion
        #region File
        /// <summary>
        /// Load file to server external storage.
        /// </summary>
        /// <returns>200: <c>IpfsHash</c> to resource.</returns>
        [HttpPost("LoadFile")]
        [RequestFormLimits(MultipartBodyLengthLimit = ValidationConst.MAX_FILE_SIZE)]
        public async Task<ActionResult<string>> LoadFile(IFormFile file)
        {
            MultipartFormDataContent content = new()
            {
                { new StreamContent(file.OpenReadStream()), file.Name, file.FileName }
            };
            HttpResponseMessage response = await _client.PostAsync(_externalStorage.PostUrl, content);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        /// <summary>
        /// Get list of files from external storage.
        /// </summary>
        /// <returns>200: json with files.</returns>
        [HttpGet("GetFiles")]
        public async Task<ActionResult<string>> GetFiles()
        {
            HttpResponseMessage response = await _client.GetAsync(_externalStorage.GetUrl);
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
        #endregion
    }
}
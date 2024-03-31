using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;

namespace ProjectTisa.Controllers.UserRelatedControllers
{
    /// <summary>
    /// Standart CRUD controller for <see cref="Notification"/> model. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, MainDbContext context, IAuthorizationService _authorizationService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IEnumerable<Notification>>> Get([FromQuery] PaginationRequest request) =>
            Ok(await request.ApplyRequest(context.Notifications.OrderBy(on => on.Id)));
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Notification>> Get(int id)
        {
            Notification? item = await context.Notifications.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            if (!item.Users.Any(x => x.Username == User.Identity!.Name) && !(await _authorizationService.AuthorizeAsync(User, "manage")).Succeeded)
            {
                return Forbid();
            }

            return Ok(item);
        }
        [HttpGet("GetAllNotifsByCurrentUser")]
        [Authorize]
        public async Task<ActionResult<Notification>> GetAllNotifsByCurrentUser() =>
            Ok((await UserUtils.GetUserFromContext(HttpContext, context)).Notifications);
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Create([FromBody] Notification item)
        {
            item.CreationTime = DateTime.UtcNow;
            context.Notifications.Add(item);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, item);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{item.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            Notification? item = await context.Notifications.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            context.Notifications.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);
            return Ok(ResAnswers.Success);
        }
        [HttpPut]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Update([FromBody] Notification item)
        {
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
    }
}

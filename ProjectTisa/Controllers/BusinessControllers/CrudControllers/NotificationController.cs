using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Controllers.GeneralData.Responses;
using ProjectTisa.Libs;
using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.Controllers.BusinessControllers.CrudControllers
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
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            if (!item.Users.Any(x => x.Username == User.Identity!.Name) && !(await _authorizationService.AuthorizeAsync(User, "manage")).Succeeded)
            {
                return Forbid();
            }

            return Ok(item);
        }
        [HttpGet("GetAllNotifsByCurrentUser")]
        [Authorize]
        public async Task<ActionResult<Notification>> GetAllNotifsByCurrentUser([FromQuery] PaginationRequest request) =>
            Ok(await request.ApplyRequest((await UserUtils.GetUserFromContext(HttpContext, context)).Notifications));
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IdResponse>> Create([FromBody] NotificationCreationReq request)
        {
            Notification notification = new(request, new(User.Identity!.Name!));
            context.Add(notification);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, notification);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{notification.Id}", new IdResponse(notification.Id));
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Delete(int id)
        {
            Notification? item = await context.Notifications.FindAsync(id);
            if (item == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            context.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);
            return Ok(new MessageResponse(ResAnswers.Success));
        }
        [HttpPut]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<MessageResponse>> Update(int id, [FromBody] NotificationCreationReq request)

        {
            Notification? toEdit = await context.Notifications.FindAsync(id);
            if (toEdit == null)
            {
                return NotFound(new MessageResponse(ResAnswers.NotFoundNullEntity));
            }

            toEdit.EditInfo.Modify(User.Identity!.Name!);
            Notification fromNotif = new(request, toEdit.EditInfo, toEdit.Id);
            context.Entry(toEdit).CurrentValues.SetValues(fromNotif);
            await context.SaveChangesAsync();
            return Ok(new MessageResponse(ResAnswers.Success));
        }
    }
}

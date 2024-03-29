using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPop.Controllers;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;

namespace ProjectTisa.Controllers.UserRelatedControllers
{
    /// <summary>
    /// Standart CRUD controller for <see cref="Notification"/> model. Mostly for testing purposes. <b>Required <see cref="AuthorizeAttribute"/> policy</b> <c>manage</c> on some actions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(ILogger<WeatherForecastController> logger, MainDbContext context, IAuthorizationService _authorizationService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<IEnumerable<Notification>>> Get([FromQuery] PaginationRequest request)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(request.ApplyRequest(await context.Notifications.OrderBy(on => on.Id).ToListAsync()));
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Notification>> Get(int id)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            Notification? item = await context.Notifications.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            if (!item.Users.Any(x => x.Username == User.Identity!.Name!) && !(await _authorizationService.AuthorizeAsync(User, "manage")).Succeeded)
            {
                return Forbid(ResAnswers.CantAccess);
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Create(Notification item)
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
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

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
        public async Task<ActionResult<string>> Update(Notification item)
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
        private bool IsTableEmpty()
        {
            return context.Notifications == null || !context.Notifications.Any();
        }
    }
}

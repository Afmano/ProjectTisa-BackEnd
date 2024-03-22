using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPop.EF.Interfaces;
using ProjectPop.Models;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;
using ProjectTisa.Models;

namespace ProjectPop.Controllers
{
    /// <summary>
    /// Standart CRUD controller for <see cref="Notification"/> model.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase, ICrud<Notification>
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> Get()
        {
            if (IsTableEmpty())
            {
                return NotFound(ResAnswers.NotFoundNullContext);
            }

            return Ok(await context.Notifications.ToListAsync());
        }
        [HttpGet("{id}")]
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

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Create(Notification item)
        {
            item.CreationTime = DateTime.UtcNow;
            context.Notifications.Add(item);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, item);

            return Created($"{HttpContext.Request.GetDisplayUrl()}/{item.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult<Notification>> Delete(int id)
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
        [Authorize(Roles = "Manager, Admin")]
        public async Task<ActionResult> Update(Notification item)
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

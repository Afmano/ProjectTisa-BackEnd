using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectPop.Models;
using ProjectTisa.Controllers.GeneralData.Requests;
using ProjectTisa.Controllers.GeneralData.Resources;
using ProjectTisa.Libs;

namespace ProjectPop.Controllers
{
    /// <summary>
    /// Test controller to check database connection and <see cref="IActionResult"/> sending.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get([FromQuery] PaginationRequest request) =>
            Ok(request.ApplyRequest(await context.WeatherForecasts.OrderBy(on => on.Id).ToListAsync()));
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherForecast>> Get(int id)
        {
            WeatherForecast? item = await context.WeatherForecasts.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            return Ok(item);
        }
        [HttpPost]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Create([FromBody] WeatherForecast item)
        {
            context.WeatherForecasts.Add(item);
            await context.SaveChangesAsync();
            LogMessageCreator.CreatedMessage(logger, item);
            return Created($"{HttpContext.Request.GetDisplayUrl()}/{item.Id}", ResAnswers.Created);
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Delete(int id)
        {
            WeatherForecast? item = await context.WeatherForecasts.FindAsync(id);
            if (item == null)
            {
                return NotFound(ResAnswers.NotFoundNullEntity);
            }

            context.WeatherForecasts.Remove(item);
            await context.SaveChangesAsync();
            LogMessageCreator.DeletedMessage(logger, item);
            return Ok(ResAnswers.Success);
        }
        [HttpPut]
        [Authorize(Policy = "manage")]
        public async Task<ActionResult<string>> Update([FromBody] WeatherForecast item)
        {
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(ResAnswers.Success);
        }
    }
}

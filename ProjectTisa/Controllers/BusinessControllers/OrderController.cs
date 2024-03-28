using Microsoft.AspNetCore.Mvc;
using ProjectPop.Controllers;

namespace ProjectTisa.Controllers.BusinessControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(ILogger<WeatherForecastController> logger, MainDbContext context) : ControllerBase
    {
    }
}

using Microsoft.AspNetCore.Mvc;
using ProjectPop.EF.Interfaces;
using ProjectPop.Models;

namespace ProjectPop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase, ICrud<WeatherForecast>
    {
        private static readonly string[] Summaries =
        [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        ];

        public ActionResult Create(WeatherForecast item)
        {
            throw new NotImplementedException();
        }

        public ActionResult<WeatherForecast> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult<WeatherForecast> Get(int id)
        {
            throw new NotImplementedException();
        }

        public ActionResult Update(WeatherForecast item)
        {
            throw new NotImplementedException();
        }

        ActionResult<IEnumerable<WeatherForecast>> ICrud<WeatherForecast>.Get()
        {
            return new OkObjectResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}

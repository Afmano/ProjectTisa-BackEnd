using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPop.Models
{
    /// <summary>
    /// Test model to check database connection using <b>WeatherForecastController</b>.
    /// </summary>
    public class WeatherForecast
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
        public override string ToString()
        {
            return $"{Id}. {Date}, {TemperatureC}°C, {TemperatureF}°F{(Summary.IsNullOrEmpty() ? "" : ", " + Summary)}";
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ProjectPop.Models;

namespace ProjectPop.EF
{
    /// <summary>
    /// Main database context used in API.
    /// </summary>
    /// <param name="options">Context options provided by app builder.</param>
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using ProjectPop.Models;

namespace ProjectPop.EF
{
    public class MainDbContext() : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using ProjectPop.Models;
using ProjectTisa.Models;

namespace ProjectPop.EF
{
    /// <summary>
    /// Main database context used in API.
    /// </summary>
    /// <param name="options">Context options provided by app builder.</param>
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<PendingRegistration> PendingRegistrations { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        }
    }
}

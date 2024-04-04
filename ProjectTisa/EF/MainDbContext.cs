using Microsoft.EntityFrameworkCore;
using ProjectTisa.Models;
using ProjectTisa.Models.BusinessLogic;

namespace ProjectTisa.EF
{
    /// <summary>
    /// Main database context used in API.
    /// </summary>
    /// <param name="options">Context options provided by app builder.</param>
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<PendingRegistration> PendingRegistrations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        #region BusinessLogic
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
        }
    }

}

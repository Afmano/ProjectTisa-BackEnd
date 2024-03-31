using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectTisa.EF;

namespace ProjectTisa.Tests.Contexts
{
    public class DatabaseContext
    {
        public static MainDbContext SetUpContext()
        {
            SqliteConnection connection = new("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<MainDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite(connection)
                .Options;
            MainDbContext context = new(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}

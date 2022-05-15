using api.Entities;

using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public class AppDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }

        public AppDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public static async Task Migrations(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<AppDBContext>();
            if (context is not null)
            {
                var conn_db = context.Database.GetDbConnection();

                Console.WriteLine($"Connection info: {conn_db.ToString()}");
                Console.WriteLine("Checking database...");

                try
                {
                    Console.WriteLine($"Identity access db: {context.Database.CanConnect()}");
                    await context.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while migrating the database. Error: {ex.Message}");
                }
            }
        }
    }
}

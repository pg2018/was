using Microsoft.EntityFrameworkCore;

namespace WebAssistedSurvey.Service.Models
{
    public class DataContext : DbContext
    {
        public DbSet<WebEvent> WebEvents { get; set; }
        public DbSet<WebSurvey> WebSurveys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WebAssistedSurvey.Service.db");
        }
    }
}
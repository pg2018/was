using Microsoft.EntityFrameworkCore;

namespace WebAssistedSurvey.Models
{
    public class SurveyContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Survey> Surveys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WebAssistedSurvey.db");
        }
    }
}
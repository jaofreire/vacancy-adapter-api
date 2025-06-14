using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Models.Logs;
using Microsoft.EntityFrameworkCore;

namespace CurriculumAdapter.API.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<FeedbackModel> Feedbacks { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<FeatureUsageLogModel> FeatureUsageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }
    }
}

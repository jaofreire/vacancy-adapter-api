using CurriculumAdapter.API.Models;
using CurriculumAdapter.API.Models.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurriculumAdapter.API.Data.Configurations
{
    public class FeatureUsageLogConfiguration : IEntityTypeConfiguration<FeatureUsageLogModel>
    {
        public void Configure(EntityTypeBuilder<FeatureUsageLogModel> builder)
        {
            builder.ToTable("feature_usage_logs");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.FeatureName)
                .HasColumnName("feature_name")
                .IsRequired();

            builder.Property(x => x.UsageDate)
                .HasColumnName("usage_date")
                .HasColumnType("DATE")
                .IsRequired();
        }
    }
}

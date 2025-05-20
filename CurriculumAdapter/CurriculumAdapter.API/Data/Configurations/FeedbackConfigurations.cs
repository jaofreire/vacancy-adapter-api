using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurriculumAdapter.API.Data.Configurations
{
    public class FeedbackConfigurations : IEntityTypeConfiguration<FeedbackModel>
    {
        public void Configure(EntityTypeBuilder<FeedbackModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();

            builder.Property(x => x.Stars)
                .HasColumnName("stars")
                .IsRequired();

            builder.Property(x => x.FeedbackText)
                .HasColumnName("feedback_text")
                .IsRequired();
        }
    }
}

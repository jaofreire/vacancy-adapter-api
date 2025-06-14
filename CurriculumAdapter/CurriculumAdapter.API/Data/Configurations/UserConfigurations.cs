using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurriculumAdapter.API.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasColumnName("first_name")
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnName("last_name")
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();
        }
    }
}

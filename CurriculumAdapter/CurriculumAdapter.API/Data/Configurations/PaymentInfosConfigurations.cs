using CurriculumAdapter.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurriculumAdapter.API.Data.Configurations
{
    public class PaymentInfosConfigurations : IEntityTypeConfiguration<PaymentInfosModel>
    {
        public void Configure(EntityTypeBuilder<PaymentInfosModel> builder)
        {
            builder.ToTable("payment_infos");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.PostalCode)
                .HasColumnName("postal_code")
                .IsRequired();

            builder.Property(x => x.Address)
                .HasColumnName("address")
                .IsRequired();

            builder.Property(x => x.AdressNumber)
                .HasColumnName("adress_number")
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired();

            builder.Property(x => x.CpfCnpj)
                .HasColumnName("cpf_cnpj")
                .IsRequired();

            builder.Property(x => x.CreditCardToken)
                .HasColumnName("credit_card_token")
                .IsRequired();  
        }
    }
}

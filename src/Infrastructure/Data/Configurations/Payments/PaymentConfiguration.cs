using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Payment;

using Microsoft.EntityFrameworkCore;
using Domain.Entities.Payment;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder
            .Property(payment => payment.SequenceNumber)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(payment => payment.RecordedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(payment => payment.RefundedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
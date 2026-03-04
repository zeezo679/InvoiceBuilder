
namespace Infrastructure.Data.Configurations.Invoice;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder
            .Property(invoice => invoice.SequenceNumber)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn();
    }
}
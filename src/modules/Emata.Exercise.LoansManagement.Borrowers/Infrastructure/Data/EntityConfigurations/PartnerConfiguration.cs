using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data.EntityConfigurations;

internal class PartnerConfiguration : IEntityTypeConfiguration<Partner>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Partner> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(e => e.Address, a =>
        {
            a.ToJson();
        });
    }
}
using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data.EntityConfigurations;

internal class BorrowerConfiguration : IEntityTypeConfiguration<Borrower>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Borrower> builder)
    {
        builder.Property(e => e.Surname)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.GivenName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.CreatedOn)
            .HasDefaultValueSql("now()");

        //add computed column for Name
        builder.Property(e => e.Name)
            .HasComputedColumnSql(""" 
                "GivenName" || ' ' || "Surname"
            """, stored: true);

        //configure Address as jsonb
        builder.OwnsOne(e => e.Address, a =>
        {
            a.ToJson();
        });
    }
}
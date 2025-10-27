using Emata.Exercise.LoansManagement.Loans.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Emata.Exercise.LoansManagement.Loans.Infrastructure.Data.EntityConfigurations;

internal class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        // configure duration to be a jsonb
        builder.OwnsOne(e => e.Duration, d =>
        {
            d.ToJson();
        });

        //configure interest to be a jsonb
        builder.OwnsOne(e => e.InterestRate, ir =>
        {
            ir.ToJson();
        });

        //set the default value for CreatedOn
        builder.Property(e => e.CreatedOn)
            .HasDefaultValueSql("now()");

    }
}

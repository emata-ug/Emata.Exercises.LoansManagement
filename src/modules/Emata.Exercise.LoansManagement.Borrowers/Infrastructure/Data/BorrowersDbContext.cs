using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;

public class BorrowersDbContext : DbContext
{
    public BorrowersDbContext(DbContextOptions<BorrowersDbContext> options)
        : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<Enum>()
         .HaveConversion<string>()
         .HaveMaxLength(50); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(BorrowersExtensions.ModuleName);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BorrowersDbContext).Assembly);
       
    // Removed HiLo sequence usage; GUID v7 IDs are generated in entity factories.
    }


    internal DbSet<Borrower> Borrowers { get; set; }

    internal DbSet<Partner> Partners { get; set; }
}

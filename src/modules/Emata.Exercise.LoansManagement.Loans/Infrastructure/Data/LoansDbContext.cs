using Emata.Exercise.LoansManagement.Loans.Domain;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Loans.Infrastructure.Data;

public class LoansDbContext : DbContext
{
    public LoansDbContext(DbContextOptions<LoansDbContext> options)
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

        modelBuilder.HasDefaultSchema(LoansExtensions.ModuleName);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoansDbContext).Assembly);
       
        //general configuration...
        modelBuilder.UseHiLo();
    }

    internal DbSet<Loan> Loans { get; set; }
}

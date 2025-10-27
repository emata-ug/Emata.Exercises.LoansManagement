using Emata.Exercise.LoansManagement.Loans.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Emata.Exercise.LoansManagement.Shared.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Emata.Exercise.LoansManagement.Loans;

public static class LoansExtensions
{

    internal const string ModuleName = "Loans";

    public static IServiceCollection AddLoansModule(this IServiceCollection services, IConfiguration configuration)
    {
        //register module services here
        services.AddEndpoints(
            assembly: typeof(LoansExtensions).Assembly);

        //database context registration
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<LoansDbContext>(options =>
        {
            options.UseNpgsql(connectionString, dbOptions =>
            {
                dbOptions.MigrationsHistoryTable("__EFMigrationsHistory", ModuleName);
            });
        });

        return services;
    }

    public static Task MigrateLoansDatabaseAsync(this IApplicationBuilder app, CancellationToken cancellationToken = default) 
        => app.MigrateDatabaseAsync<LoansDbContext>(cancellationToken);
}

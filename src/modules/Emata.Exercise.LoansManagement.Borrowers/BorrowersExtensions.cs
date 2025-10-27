using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Emata.Exercise.LoansManagement.Shared.Infrastructure;

namespace Emata.Exercise.LoansManagement.Borrowers;

public static class BorrowersExtensions
{

    internal const string ModuleName = "Borrowers";

    public static IServiceCollection AddBorrowersModule(this IServiceCollection services, IConfiguration configuration)
    {
        //register module services here
        services.AddEndpoints(
            assembly: typeof(BorrowersExtensions).Assembly);

        //database context registration
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<BorrowersDbContext>(options =>
        {
            options.UseNpgsql(connectionString, dbOptions =>
            {
                dbOptions.MigrationsHistoryTable("__EFMigrationsHistory", ModuleName);
            });
        });

        return services;
    }

    public static Task MigrateBorrowersDatabaseAsync(this IApplicationBuilder app, CancellationToken cancellationToken = default) 
        => app.MigrateDatabaseAsync<BorrowersDbContext>(cancellationToken);
}

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Emata.Exercise.LoansManagement.Shared.Infrastructure;

public static class EntityCoreExtensions
{
    public static async Task MigrateDatabaseAsync<TDbContext>(this IApplicationBuilder app, CancellationToken cancellationToken = default) where TDbContext : DbContext
    {
        using IServiceScope? scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider; // Use the scoped service provider

        var options = serviceProvider.GetRequiredService<DbContextOptions<TDbContext>>();

        TDbContext? context = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider, options);
        await context.Database.MigrateAsync(cancellationToken);
    }
}

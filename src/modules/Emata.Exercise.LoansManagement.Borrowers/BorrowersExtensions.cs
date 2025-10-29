using System.Reflection;
using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Emata.Exercise.LoansManagement.Shared.Infrastructure;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Emata.Exercise.LoansManagement.Borrowers.UseCases.Partners;
using Emata.Exercise.LoansManagement.Shared;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Borrowers.UseCases.Borrowers;
using Emata.Exercise.LoansManagement.Contracts.Borrowers;
using Emata.Exercise.LoansManagement.Borrowers.Presentation;

namespace Emata.Exercise.LoansManagement.Borrowers;

public static class BorrowersExtensions
{

    internal const string ModuleName = "Borrowers";

    public static IServiceCollection AddBorrowersModule(this IServiceCollection services,
        IConfiguration configuration,
        List<Assembly> mediatorAssemblies)
    {
        //register module assembly once for all handlers
        mediatorAssemblies.Add(typeof(BorrowersExtensions).Assembly);

        //database context registration
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<BorrowersDbContext>(options =>
        {
            options.UseNpgsql(connectionString, dbOptions =>
            {
                dbOptions.MigrationsHistoryTable("__EFMigrationsHistory", ModuleName);
            });
        });

        // application services & handlers
        services.AddScoped<ICommandHandler<AddBorrowerCommand, BorrowerDTO>, AddBorrowerCommandHandler>();
        services.AddScoped<IQueryHandler<GetBorrowerByIdQuery, BorrowerDTO?>, GetBorrowerByIdQueryHandler>();
        services.AddScoped<IQueryHandler<GetBorrowerSummariesQuery, List<BorrowerSummaryDTO>>, GetBorrowerSummariesQueryHandler>();
        services.AddScoped<ICommandHandler<AddPartnerCommand, PartnerDTO>, AddPartnerCommandHandler>();
        services.AddScoped<IQueryHandler<GetPartnersQuery, List<PartnerDTO>>, GetPartnersQueryHandler>();
        services.AddScoped<IBorrowerService, BorrowerService>();

        //register endpoints...
        services.AddEndpoints(typeof(BorrowersExtensions).Assembly);

        return services;
    }

    public static Task MigrateBorrowersDatabaseAsync(this IApplicationBuilder app, CancellationToken cancellationToken = default) 
        => app.MigrateDatabaseAsync<BorrowersDbContext>(cancellationToken);
}

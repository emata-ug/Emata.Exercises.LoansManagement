using Emata.Exercise.LoansManagement.Loans.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Loans;

public class LoansEndpoints : IEndpoints
{
    public string? Prefix => "loans";

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet($"health", () => Results.Ok("Loans API is healthy"))
        .WithSummary("Get Health")
        .WithDescription("Checks the health of the Loans API");

        //query for loans
        app.MapGet($"", async (LoansDbContext dbContext) =>
        {
            var loans = await dbContext.Loans
                .ToListAsync();
            return Results.Ok(loans);
        })
        .WithName("Get All Loans")
        .WithSummary("Get all loans")
        .WithDescription("Retrieves a list of all loans in the system.");

        //query for a specific loan
        app.MapGet($"{{id:int}}", async (int id, LoansDbContext dbContext) =>
        {
            var loan = await dbContext.Loans
                .FirstOrDefaultAsync(l => l.Id == id);
            return loan is not null ? Results.Ok(loan) : Results.NotFound();
        })
        .WithName("Get Loan By ID")
        .WithSummary("Get loan by ID")
        .WithDescription("Retrieves a specific loan by its ID.");
    }
}

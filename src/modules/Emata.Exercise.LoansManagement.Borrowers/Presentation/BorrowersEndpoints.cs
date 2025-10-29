using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Emata.Exercise.LoansManagement.Shared;

namespace Emata.Exercise.LoansManagement.Borrowers.Presentation;

public class BorrowersEndpoints : IEndpoints
{
    public string? Prefix => "borrowers";

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        //health check endpoint
        app.MapGet($"health", () => Results.Ok("Borrowers API is healthy"))
            .WithSummary("Get Health")
            .WithDescription("Checks the health of the Borrowers API");

        //add borrower
        app.MapPost($"", async (
            [FromServices] ICommandHandler<AddBorrowerCommand, BorrowerDTO> handler,
            [FromBody] AddBorrowerCommand command) =>
        {
            var borrower = await handler.Handle(command);
            return Results.Created($"/borrowers/{borrower.Id}", borrower);
        })
        .WithSummary("Add Borrower")
        .WithDescription("Adds a new borrower to the system");

        // get borrower summaries
            app.MapGet($"summaries", async (
                [FromServices] IQueryHandler<GetBorrowerSummariesQuery, List<BorrowerSummaryDTO>> handler,
                [AsParameters] GetBorrowerSummariesQuery query) =>
            {
                var borrowerSummaries = await handler.Handle(query);
                return Results.Ok(borrowerSummaries);
            })
        .WithSummary("Get Borrower Summaries")
        .WithDescription("Retrieves summaries of borrowers based on provided criteria.");
        
        //get borrower by id
            app.MapGet($"{{id:int}}", async (int id, IQueryHandler<GetBorrowerByIdQuery, BorrowerDTO?> handler) =>
            {
                var borrower = await handler.Handle(new GetBorrowerByIdQuery(id));
                return borrower is not null ? Results.Ok(borrower) : Results.NotFound();
            })
        .WithSummary("Get Borrower By ID")
        .WithDescription("Retrieves a borrower by their unique ID.");
    }
}

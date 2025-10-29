using Emata.Exercise.LoansManagement.Borrowers.UseCases.Partners;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Emata.Exercise.LoansManagement.Borrowers.Presentation;

internal class PartnersEndpoints : IEndpoints
{
    public string? Prefix => "partners";

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        //health check endpoint
        app.MapGet($"health", () => Results.Ok("Partners API is healthy"))
        .WithSummary("Get Health")
        .WithDescription("Checks the health of the Partners API");

        //add more endpoints as needed
        app.MapPost($"", async (
            [FromBody] AddPartnerCommand command,
            [FromServices]ICommandHandler<AddPartnerCommand, PartnerDTO> handler) =>
        {
            var partner = await handler.Handle(command);
            return Results.Created(string.Empty, partner);
        })
        .WithSummary("Add Partner")
        .WithDescription("Adds a new partner to the system.");
        
        //get all partners
        app.MapGet($"", async (
            [FromServices] IQueryHandler<GetPartnersQuery, List<PartnerDTO>> handler) =>
        {
            var partners = await handler.Handle(new GetPartnersQuery());
            return Results.Ok(partners);
        })
        .WithSummary("Get All Partners")
        .WithDescription("Retrieves all partners from the system.");
    }
}

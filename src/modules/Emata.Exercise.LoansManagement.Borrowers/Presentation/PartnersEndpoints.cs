using System;
using Emata.Exercise.LoansManagement.Shared.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Emata.Exercise.LoansManagement.Borrowers.Presentation;

public class PartnersEndpoints : IEndpoints
{
    public string? Prefix => "partners";

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        //health check endpoint
        app.MapGet($"health", () => Results.Ok("Partners API is healthy"))
        .WithSummary("Get Health")
        .WithDescription("Checks the health of the Partners API");

    }
}

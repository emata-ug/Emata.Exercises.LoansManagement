using Microsoft.AspNetCore.Routing;

namespace Emata.Exercise.LoansManagement.Shared.Endpoints;

/// <summary>
/// Implementation inspired by https://www.milanjovanovic.tech/blog/automatically-register-minimal-apis-in-aspnetcore
/// </summary>
public interface IEndpoints
{
    string? Prefix { get; }
    void MapEndpoints(IEndpointRouteBuilder app);
}   
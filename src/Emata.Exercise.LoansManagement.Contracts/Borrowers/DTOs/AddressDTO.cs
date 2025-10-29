namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record class AddressDTO
{
    public required string Town { get; init; }

    public string? District { get; init; }

    public string? Region { get; init; }

    public string? Country { get; init; }
}
namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record BorrowerSummaryDTO
{
    public Guid Id { get; init; }

    public required string Name { get; init; }

    public Gender Gender { get; init; }

    public string? PartnerName { get; init; }

    public string? PhoneNumber { get; init; }

    public string? Town { get; init; }

}
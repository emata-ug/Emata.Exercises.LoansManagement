namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record GetBorrowerSummariesQuery : IQuery<List<BorrowerSummaryDTO>>
{
    public Guid[]? PartnerIds { get; init; }

    public Guid[]? BorrowerIds { get; set; }
}
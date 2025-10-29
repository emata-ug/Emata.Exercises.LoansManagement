namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record GetBorrowerSummariesQuery : IQuery<List<BorrowerSummaryDTO>>
{
    public int[]? PartnerIds { get; init; }

    public int[]? BorrowerIds { get; set; }
}
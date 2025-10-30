using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Borrowers.UseCases.Borrowers;

internal class GetBorrowerSummariesQueryHandler : IQueryHandler<GetBorrowerSummariesQuery, List<BorrowerSummaryDTO>>
{
    private readonly BorrowersDbContext _dbContext;
    
    public GetBorrowerSummariesQueryHandler(BorrowersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<BorrowerSummaryDTO>> Handle(GetBorrowerSummariesQuery query, CancellationToken cancellationToken)
    {
        var borrowers = await _dbContext.Borrowers
            .Where(b => query.BorrowerIds == null || query.BorrowerIds.Contains(b.Id))
            .Where(b => query.PartnerIds == null || query.PartnerIds.Contains(b.PartnerId))
            .Select(b => b.ToSummaryDTO())
            .ToListAsync(cancellationToken);

        return borrowers;
    }
}

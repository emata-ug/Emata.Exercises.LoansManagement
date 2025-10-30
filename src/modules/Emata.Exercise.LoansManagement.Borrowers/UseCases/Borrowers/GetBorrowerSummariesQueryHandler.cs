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
        var borrowersQuery = _dbContext.Borrowers.AsQueryable();

        if (query.BorrowerIds != null && query.BorrowerIds.Length > 0)
        {
            borrowersQuery = borrowersQuery.Where(b => query.BorrowerIds.Contains(b.Id));
        }

        if (query.PartnerIds != null && query.PartnerIds.Length > 0)
        {
            borrowersQuery = borrowersQuery.Where(b => query.PartnerIds.Contains(b.PartnerId));
        }

        var borrowers = await borrowersQuery
            .Select(b => b.ToSummaryDTO())
            .ToListAsync(cancellationToken);

        return borrowers;
    }
}

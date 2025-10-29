using System.Threading;
using System.Threading.Tasks;
using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Borrowers.UseCases.Borrowers;

internal class GetBorrowerByIdQueryHandler  : IQueryHandler<GetBorrowerByIdQuery, BorrowerDTO?>
{
    private readonly BorrowersDbContext _dbContext;

    public GetBorrowerByIdQueryHandler(BorrowersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BorrowerDTO?> Handle(GetBorrowerByIdQuery request, CancellationToken cancellationToken)
    {
        var borrower = await _dbContext.Borrowers
            .FirstOrDefaultAsync(b => b.Id == request.BorrowerId, cancellationToken);

        return borrower?.ToDTO();
    }
}

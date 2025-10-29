using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;

namespace Emata.Exercise.LoansManagement.Borrowers.Presentation;

internal class BorrowerService(IQueryHandler<GetBorrowerByIdQuery, BorrowerDTO?> getBorrowerByIdQueryHandler) : IBorrowerService
{
    private readonly IQueryHandler<GetBorrowerByIdQuery, BorrowerDTO?> _getBorrowerByIdQueryHandler = getBorrowerByIdQueryHandler;

    public Task<BorrowerDTO?> GetBorrowerByIdAsync(Guid borrowerId, CancellationToken cancellationToken = default)
    {
        return _getBorrowerByIdQueryHandler.Handle(new GetBorrowerByIdQuery(borrowerId), cancellationToken);
    }
}

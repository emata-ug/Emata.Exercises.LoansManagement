using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Contracts.Shared;
using Emata.Exercise.LoansManagement.Shared;

namespace Emata.Exercise.LoansManagement.Borrowers.UseCases.Partners;

internal class GetPartnersQueryHandler : IQueryHandler<GetPartnersQuery, List<PartnerDTO>>
{
    public readonly BorrowersDbContext _dbContext;
    public GetPartnersQueryHandler(BorrowersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<PartnerDTO>> Handle(GetPartnersQuery request, CancellationToken cancellationToken)
    {
        var partners = _dbContext.Partners.Select(p => p.ToDTO());
        return Task.FromResult(partners.ToList());
    }
}
internal record GetPartnersQuery : IQuery<List<PartnerDTO>>;
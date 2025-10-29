using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;

namespace Emata.Exercise.LoansManagement.Borrowers.UseCases.Partners;

internal class AddPartnerCommandHandler : ICommandHandler<AddPartnerCommand, PartnerDTO>
{
    private readonly BorrowersDbContext _dbContext;

    public AddPartnerCommandHandler(BorrowersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<PartnerDTO> Handle(AddPartnerCommand command, CancellationToken cancellationToken = default)
    {
        var partner = Domain.Partner.Create(
            command.Name,
            command.Town);

        _dbContext.Partners.Add(partner);
        _dbContext.SaveChanges();

        return Task.FromResult(partner.ToDTO());
    }
}


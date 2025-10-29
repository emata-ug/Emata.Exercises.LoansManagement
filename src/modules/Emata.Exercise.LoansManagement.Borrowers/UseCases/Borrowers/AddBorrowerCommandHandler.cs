using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Emata.Exercise.LoansManagement.Borrowers.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Shared;


namespace Emata.Exercise.LoansManagement.Borrowers.UseCases.Borrowers;

internal class AddBorrowerCommandHandler : ICommandHandler<AddBorrowerCommand, BorrowerDTO>
{
    private readonly BorrowersDbContext _dbContext;

    public AddBorrowerCommandHandler(BorrowersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BorrowerDTO> Handle(AddBorrowerCommand request, CancellationToken cancellationToken = default)
    {
        //get partner by Id
        var partner = await _dbContext.Partners.FindAsync(request.PartnerId, cancellationToken);
        if (partner == null)
        {
            throw new Exception($"Partner with Id {request.PartnerId} not found."); //In real scenario, use a custom exception
        }

        var borrower = BorrowerBuilder.Create()
            .SetSurname(request.Surname)
            .SetGivenName(request.GivenName)
            .SetPhoneNumber(request.PhoneNumber)
            .SetEmail(request.Email)
            .SetDateOfBirth(request.DateOfBirth)
            .SetIdentificationNumber(request.IdentificationNumber)
            .SetPartnerId(request.PartnerId)
            .SetTown(request.Town)
            .Build();

        _dbContext.Borrowers.Add(borrower);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return borrower.ToDTO();
    }
}

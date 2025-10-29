using Emata.Exercise.LoansManagement.Contracts.Borrowers;
using Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;
using Emata.Exercise.LoansManagement.Loans.Domain;
using Emata.Exercise.LoansManagement.Loans.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Shared;
using Microsoft.Extensions.Logging;

namespace Emata.Exercise.LoansManagement.Loans.UseCases;

internal class AddLoanCommandHandler : ICommandHandler<AddLoanCommand, LoanItem>
{

    private readonly ILogger<AddLoanCommandHandler> _logger;
    private readonly IBorrowerService _borrowerQueryService;
    private readonly LoansDbContext _dbContext;
    

    public AddLoanCommandHandler(LoansDbContext dbContext, ILogger<AddLoanCommandHandler> logger, IBorrowerService borrowerQueryService)
    {
        _borrowerQueryService = borrowerQueryService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<LoanItem> Handle(AddLoanCommand request, CancellationToken cancellationToken = default)
    {
        // Check borrower exists
    var borrower = await _borrowerQueryService.GetBorrowerByIdAsync(request.BorrowerId, cancellationToken);

        if (borrower is null)
        {
            _logger.LogWarning("Borrower {BorrowerId} not found when trying to create a loan", request.BorrowerId);
            throw new Exception($"Borrower with ID {request.BorrowerId} not found.");
        }

        _logger.LogInformation("Creating loan for borrower {BorrowerId}", request.BorrowerId);
        
        var loan = Loan.Create(
            request.BorrowerId,
            request.LoanAmount,
            request.IssueDate,
            InterestRate.Create(
                request.InterestRate.PercentageRate,
                request.InterestRate.Period),
            Duration.Create(
                request.Duration.Length,
                request.Duration.Period),
            request.Reference,
            request.Reason);

        _dbContext.Loans.Add(loan);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loan {LoanId} created for borrower {BorrowerId}", loan.Id, loan.BorrowerId);

        var loanItem = loan.ToDTO();

        return loanItem;
    }
}
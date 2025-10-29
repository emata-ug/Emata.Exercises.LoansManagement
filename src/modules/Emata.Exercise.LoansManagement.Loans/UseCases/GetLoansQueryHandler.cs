using Emata.Exercise.LoansManagement.Contracts.Loans;
using Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;
using Emata.Exercise.LoansManagement.Loans.Infrastructure.Data;
using Emata.Exercise.LoansManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace Emata.Exercise.LoansManagement.Loans.UseCases;

internal class GetLoansQueryHandler : IQueryHandler<GetLoansQuery, List<LoanItem>>
{
    private readonly LoansDbContext _dbContext;

    public GetLoansQueryHandler(LoansDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<LoanItem>> Handle(GetLoansQuery request, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Loans.AsQueryable();
        if (request.BorrowerIds != null && request.BorrowerIds.Length != 0)
        {
            query = query.Where(loan => request.BorrowerIds.Contains(loan.BorrowerId));
        }

        if (request.MinLoanAmount.HasValue)
        {
            query = query.Where(loan => loan.LoanAmount >= request.MinLoanAmount.Value);
        }

        if (request.MaxLoanAmount.HasValue)
        {
            query = query.Where(loan => loan.LoanAmount <= request.MaxLoanAmount.Value);
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(loan => loan.IssueDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(loan => loan.IssueDate <= request.EndDate.Value);
        }

        var loans = await query.Select(loan => new LoanItem
        {
            Id = loan.Id,
            BorrowerId = loan.BorrowerId,
            LoanAmount = loan.LoanAmount,
            IssueDate = loan.IssueDate,
            Duration = loan.Duration != null ? new DurationDto
            {
                Length = loan.Duration.Length,
                Period = loan.Duration.Period
            } : null,
            InterestRate = new InterestRateDto
            {
                PercentageRate = loan.InterestRate.PercentageRate,
                Period = loan.InterestRate.Period
            }
        })
        .OrderByDescending(l => l.IssueDate)
        .Take(1000)
        .ToListAsync(cancellationToken);


        return loans;
    }
}

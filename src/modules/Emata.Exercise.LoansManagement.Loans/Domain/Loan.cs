using System;
using Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;

namespace Emata.Exercise.LoansManagement.Loans.Domain;

internal class Loan
{
    public int Id { get; private set; }

    public int BorrowerId { get; private set; }

    public decimal LoanAmount { get; private set; }

    public DateOnly IssueDate { get; private set; }

    public InterestRate InterestRate { get; private set; } = null!;

    public Duration?    Duration { get; private set; }

    public string? Reference { get; private set; }

    public string? Reason { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public static Loan Create(
        int borrowerId,
        decimal loanAmount,
        DateOnly issueDate,
        InterestRate interestRate,
        Duration duration,
        string? reference,
        string? reason) => new Loan
        {
            BorrowerId = borrowerId,
            LoanAmount = loanAmount,
            IssueDate = issueDate,
            InterestRate = interestRate,
            Duration = duration,
            Reference = reference,
            Reason = reason,
            CreatedOn = DateTime.UtcNow
        };


}

internal class Duration
{
    public int Length { get; private set; }

    public Period Period { get; private set; }

    public static Duration Create(int length, Period period) => new()
    {
        Length = length,
        Period = period
    };
}

internal class InterestRate
{
    public decimal PercentageRate { get; private set; }

    public Period Period { get; private set; }

    public static InterestRate Create(decimal percentageRate, Period period) => new()
    {
        PercentageRate = percentageRate,
        Period = period
    };
}

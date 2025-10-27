using System;

namespace Emata.Exercise.LoansManagement.Loans.Domain;

internal class Loan
{
    public int Id { get; set; }

    public decimal LoanAmount { get; set; }

    public DateOnly IssueDate { get; set; }

    public required InterestRate InterestRate { get; set; }

    public Duration? Duration { get; set; }

    public string? LoanApplicationId { get; set; }

    public DateTime CreatedOn { get; set; }

    
}

internal class Duration
{
    public int Length { get; set; }

    public Period Period { get; set; }
}

internal class InterestRate
{
    public decimal Rate { get; set; }

    public Period Period { get; set; }
}

public enum Period
{
    Annual = 0,
    Monthly = 1,
    Weekly = 2,
    Daily = 3
}
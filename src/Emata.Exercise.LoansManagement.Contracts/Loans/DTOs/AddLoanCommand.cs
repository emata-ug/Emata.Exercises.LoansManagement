namespace Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;

public record AddLoanCommand : ICommand<LoanItem>
{
    public Guid BorrowerId { get; set; }

    public decimal LoanAmount { get; set; }

    public DateOnly IssueDate { get; set; }

    public string? Reference { get; set; }
    
    public string? Reason { get; set; }

    public int MyProperty { get; set; }

    public DurationDto Duration { get; set; } = new DurationDto();

    public InterestRateDto InterestRate { get; set; } = new InterestRateDto();
}
namespace Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;

public record InterestRateDto
{
    public decimal PercentageRate { get; set; }

    public Period Period { get; set; }
}

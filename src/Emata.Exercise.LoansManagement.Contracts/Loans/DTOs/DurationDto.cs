namespace Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;

public record DurationDto
{
    public int Length { get; set; }

    public Period Period { get; set; }
}

namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Address
{
    public required string Town { get; set; }

    public string? District { get; set; }

    public string? Region { get; set; }

    public string? Country { get; set; }
}
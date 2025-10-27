namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Borrower
{
    public int Id { get; set; }

    public required string Surname { get; set; }

    public required string GivenName { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    public string? IdentificationNumber { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public DateTime CreatedOn { get; set; }

    public Address? Address { get; set; }

    public int PartnerId { get; set; }
    public Partner? Partner { get; set; }
}
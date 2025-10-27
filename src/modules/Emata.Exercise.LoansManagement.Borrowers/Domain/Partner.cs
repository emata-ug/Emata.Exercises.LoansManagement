namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Partner
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public Address? Address { get; set; }
}

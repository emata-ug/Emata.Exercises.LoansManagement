namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Partner
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public Address? Address { get; private set; }

    public static Partner Create(string name, string? town)
    {
        return new Partner
        {
            Name = name,
            Address = town is not null ? Address.Create(town) : null
        };
    }
}

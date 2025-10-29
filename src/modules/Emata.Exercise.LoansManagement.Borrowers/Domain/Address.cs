namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Address
{
    public string Town { get; private set; } = string.Empty;

    public string? District { get; private set; }

    public string? Region { get; private set; }

    public string? Country { get; private set; }

    public static Address Create(string town)
    {
        return new Address
        {
            Town = town
        };
    }

    public static Address Create(string town,
        string? district,
        string? region,
        string? country)
    {
        return new Address
        {
            Town = town,
            District = district,
            Region = region,
            Country = country
        };
    }
}
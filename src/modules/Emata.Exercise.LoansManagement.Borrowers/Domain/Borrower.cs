using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

internal class Borrower
{
    public int Id { get; private set; }

    public string Surname { get; private set; } = string.Empty;

    public string GivenName { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public Gender Gender { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public string? IdentificationNumber { get; private set; }

    public string PhoneNumber { get; private set; } = string.Empty;

    public string? Email { get; private set; }

    public DateTime CreatedOn { get; private set; }

    public Address? Address { get; private set; }

    public int PartnerId { get; private set; }
    public Partner? Partner { get; private set; }

    public static Borrower Create(
        string surname,
        string givenName,
        Gender gender,
        DateOnly dateOfBirth,
        string phoneNumber,
        DateTime createdOn,
        string? town,
        int partnerId,
        string? identificationNumber = null,
        string? email = null)
    {
        var borrower = new Borrower
        {
            Surname = surname,
            GivenName = givenName,
            Gender = gender,
            Name = $"{givenName} {surname}",
            DateOfBirth = dateOfBirth,
            PhoneNumber = phoneNumber,
            CreatedOn = createdOn,
            PartnerId = partnerId,
            IdentificationNumber = identificationNumber,
            Email = email,
            Address = town is not null
                ? Address.Create(town)
                : null
        };

        return borrower;
    }
}
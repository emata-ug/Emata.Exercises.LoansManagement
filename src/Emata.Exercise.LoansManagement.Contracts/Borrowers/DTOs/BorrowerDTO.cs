namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record BorrowerDTO
{
    public Guid Id { get; init; }

    public required string Surname { get; init; }

    public required string GivenName { get; init; }

    public string Name { get; init; } = string.Empty;

    public Gender Gender { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string? IdentificationNumber { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Email { get; init; }

    public DateTime CreatedOn { get; init; }

    public AddressDTO? AddressDTO { get; init; }

    public PartnerDTO? PartnerDTO { get; init; }
}
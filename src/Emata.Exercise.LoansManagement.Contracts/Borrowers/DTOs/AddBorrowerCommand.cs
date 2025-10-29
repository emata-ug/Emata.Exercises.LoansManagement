using Emata.Exercise.LoansManagement.Contracts.Shared;

namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record AddBorrowerCommand : ICommand<BorrowerDTO>
{
    public required string Surname { get; init; }

    public required string GivenName { get; init; }

    public required Gender Gender { get; init; }

    public DateOnly DateOfBirth { get; init; }

    public string? IdentificationNumber { get; init; }

    public required string PhoneNumber { get; init; }

    public string? Email { get; init; }

    public string? Town { get; init; }

    public int PartnerId { get; init; }
}
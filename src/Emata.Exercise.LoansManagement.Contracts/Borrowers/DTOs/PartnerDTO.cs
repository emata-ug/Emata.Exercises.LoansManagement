namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record class PartnerDTO
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public AddressDTO? Address { get; set; }
}
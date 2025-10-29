namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record class PartnerDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public AddressDTO? AddressDTO { get; set; }
}
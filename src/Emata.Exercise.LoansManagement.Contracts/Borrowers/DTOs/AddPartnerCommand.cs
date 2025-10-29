using Emata.Exercise.LoansManagement.Contracts.Shared;

namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record class AddPartnerCommand : ICommand<PartnerDTO>
{
    public required string Name { get; set; }

    public string? Town { get; set; }
}
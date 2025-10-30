using System.ComponentModel.DataAnnotations;
using Emata.Exercise.LoansManagement.Contracts.Shared;

namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record class AddPartnerCommand : ICommand<PartnerDTO>
{
    [Required(ErrorMessage = "Name is required."), StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
    public required string Name { get; set; }

    public string? Town { get; set; }
}
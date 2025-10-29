namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record GetBorrowerByIdQuery(Guid BorrowerId) : IQuery<BorrowerDTO?>;
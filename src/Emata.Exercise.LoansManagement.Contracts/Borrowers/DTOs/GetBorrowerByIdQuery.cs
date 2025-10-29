namespace Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

public record GetBorrowerByIdQuery(int BorrowerId) : IQuery<BorrowerDTO?>;
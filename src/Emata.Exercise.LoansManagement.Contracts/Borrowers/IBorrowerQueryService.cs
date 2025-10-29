using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Contracts.Borrowers;

public interface IBorrowerService
{
    Task<BorrowerDTO?> GetBorrowerByIdAsync(int borrowerId, CancellationToken cancellationToken = default);
}

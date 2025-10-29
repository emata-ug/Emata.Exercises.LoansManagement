using System.Threading;
using System.Threading.Tasks;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Contracts.Borrowers;

public interface IBorrowerQueryService
{
    Task<BorrowerDTO?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
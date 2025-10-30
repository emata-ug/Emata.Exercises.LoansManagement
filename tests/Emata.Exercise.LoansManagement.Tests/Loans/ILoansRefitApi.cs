using Emata.Exercise.LoansManagement.Contracts.Loans;
using Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;
using Refit;

namespace Emata.Exercise.LoansManagement.Tests.Loans;

public interface ILoansRefitApi
{
    [Post("/loans")]
    Task<ApiResponse<LoanItem>> AddLoanAsync([Body] AddLoanCommand loan);

    [Get("/loans")]
    Task<ApiResponse<List<LoanItem>>> GetLoansAsync([Query] GetLoansQuery query);

    [Get("/loans/{id}")]
    Task<ApiResponse<LoanItem>> GetLoanByIdAsync(Guid id);
}

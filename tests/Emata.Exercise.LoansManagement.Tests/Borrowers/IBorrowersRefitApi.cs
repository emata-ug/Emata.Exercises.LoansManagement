using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Refit;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

public interface IBorrowersRefitApi
{
    [Post("/partners")]
    Task<ApiResponse<PartnerDTO>> AddPartnerAsync(AddPartnerCommand partner);

    [Get("/partners")]
    Task<ApiResponse<List<PartnerDTO>>> GetPartnersAsync();

    [Post("/borrowers")]
    Task<ApiResponse<BorrowerDTO>> AddBorrowerAsync(AddBorrowerCommand borrower);

    [Get("/borrowers")]
    Task<ApiResponse<List<BorrowerDTO>>> GetBorrowersAsync(GetBorrowerSummariesQuery query);

    [Get("/borrowers/{id}")]
    Task<ApiResponse<BorrowerDTO>> GetBorrowerByIdAsync(Guid id);
}

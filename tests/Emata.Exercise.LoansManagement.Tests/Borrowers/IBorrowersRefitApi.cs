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
    Task<ApiResponse<BorrowerDTO>> AddBorrowerAsync([Body] AddBorrowerCommand borrower);

    [Get("/borrowers/summaries")]
    Task<ApiResponse<List<BorrowerSummaryDTO>>> GetBorrowersAsync([Query(CollectionFormat.Multi)] Guid[]? partnerIds = null, [Query(CollectionFormat.Multi)] Guid[]? borrowerIds = null);

    [Get("/borrowers/{id}")]
    Task<ApiResponse<BorrowerDTO>> GetBorrowerByIdAsync(Guid id);
}

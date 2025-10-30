using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

[Collection(BorrowersCollectionFixture.CollectionName)]
public class GetBorrowerSummariesTests : IAsyncLifetime
{
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private List<BorrowerSummaryDTO> _borrowers = [];
    private PartnerDTO _partner = default!;

    public GetBorrowerSummariesTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
    {
        _borrowersApi = apiFactory.BorrowersApi;
        _testOutputHelper = testOutputHelper;
        _resetDatabaseAsync = apiFactory.ResetDatabaseAsync;
    }

    public Task DisposeAsync()
    {
        return _resetDatabaseAsync();
    }

    public async Task InitializeAsync()
    {
        // Create a partner
        var addPartnerCommand = BorrowerFakers.AddPartnerCommandFaker.Generate();
        var partnerResponse = await _borrowersApi.AddPartnerAsync(addPartnerCommand);
        await partnerResponse.EnsureSuccessfulAsync();
        _partner = partnerResponse.Content!;

        // Create multiple borrowers
        var borrowerRequests = BorrowerFakers.AddBorrowerCommandFaker.Generate(new Random().Next(5, 10));
        foreach (var borrowerRequest in borrowerRequests)
        {
            var request = borrowerRequest with { PartnerId = _partner.Id };
            var response = await _borrowersApi.AddBorrowerAsync(request);
            await response.EnsureSuccessfulAsync();
            // Convert to BorrowerSummaryDTO for tracking
            _borrowers.Add(new BorrowerSummaryDTO
            {
                Id = response.Content!.Id,
                Name = response.Content.Name,
                Gender = response.Content.Gender,
                PartnerName = _partner.Name,
                PhoneNumber = response.Content.PhoneNumber,
                Town = response.Content.AddressDTO?.Town
            });
        }
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldReturnAllBorrowers()
    {
        // Act
        var response = await _borrowersApi.GetBorrowersAsync();

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.Count.ShouldBeGreaterThanOrEqualTo(_borrowers.Count);

        _testOutputHelper.WriteLine("Retrieved {0} borrower summaries", response.Content.Count);
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldFilterByPartnerId()
    {
        // Act
        var response = await _borrowersApi.GetBorrowersAsync(partnerIds: [_partner.Id]);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.Count.ShouldBe(_borrowers.Count);
        
        // Compare that returned borrowers match expected borrowers
        var expectedIds = _borrowers.Select(b => b.Id).OrderBy(id => id).ToList();
        var actualIds = response.Content.Select(b => b.Id).OrderBy(id => id).ToList();
        actualIds.ShouldBe(expectedIds);

        _testOutputHelper.WriteLine("Retrieved {0} borrowers for partner {1}", response.Content.Count, _partner.Id);
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldFilterByBorrowerIds()
    {
        // Arrange
        var borrowerIds = _borrowers.Take(3).Select(b => b.Id).ToArray();

        // Act
        var response = await _borrowersApi.GetBorrowersAsync(borrowerIds: borrowerIds);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.Count.ShouldBe(borrowerIds.Length);
        response.Content.ShouldAllBe(b => borrowerIds.Contains(b.Id));
        
        // Compare that returned borrowers match the requested borrower IDs
        var expectedIds = borrowerIds.OrderBy(id => id).ToList();
        var actualIds = response.Content.Select(b => b.Id).OrderBy(id => id).ToList();
        actualIds.ShouldBe(expectedIds);

        _testOutputHelper.WriteLine("Retrieved {0} borrowers by IDs", response.Content.Count);
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldReturnEmptyListForNonExistentPartner()
    {
        // Act
        var response = await _borrowersApi.GetBorrowersAsync(partnerIds: [Guid.NewGuid()]);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldBeEmpty();

        _testOutputHelper.WriteLine("Retrieved 0 borrowers for non-existent partner");
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldReturnEmptyListForNonExistentBorrowerIds()
    {
        // Act
        var response = await _borrowersApi.GetBorrowersAsync(borrowerIds: [Guid.NewGuid(), Guid.NewGuid()]);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldBeEmpty();

        _testOutputHelper.WriteLine("Retrieved 0 borrowers for non-existent borrower IDs");
    }

    [Fact]
    public async Task GetBorrowerSummaries_ShouldHandleMultipleFilters()
    {
        // Arrange
        var borrowerIds = _borrowers.Take(2).Select(b => b.Id).ToArray();

        // Act
        var response = await _borrowersApi.GetBorrowersAsync(partnerIds: [_partner.Id], borrowerIds: borrowerIds);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.Count.ShouldBe(borrowerIds.Length);

        _testOutputHelper.WriteLine("Retrieved {0} borrowers with multiple filters", response.Content.Count);
    }
}

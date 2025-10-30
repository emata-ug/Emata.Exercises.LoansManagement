using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers.Partners;

[Collection(BorrowersCollectionFixture.CollectionName)]
public class GetPartnerTests : IAsyncLifetime
{
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private List<PartnerDTO> _partners = [];

    public GetPartnerTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
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
        //generate random number of partners...
        var newPartnerRequests = BorrowerFakers.AddPartnerCommandFaker.Generate(new Random().Next(5, 15));
        foreach (var newPartnerRequest in newPartnerRequests)
        {
            var response = await _borrowersApi.AddPartnerAsync(newPartnerRequest);
            await response.EnsureSuccessfulAsync();
            _partners.Add(response.Content!);
        }
    }

    [Fact]
    public async Task GetPartners_Should_ReturnAllPartners()
    {
        // Act
        var response = await _borrowersApi.GetPartnersAsync();

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldBe(_partners, ignoreOrder: true);
    }
}

using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

[Collection(BorrowersCollectionFixture.CollectionName)]
public class GetBorrowerByIdTests : IAsyncLifetime
{
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private BorrowerDTO _borrower = default!;

    public GetBorrowerByIdTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
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

        // Create a borrower
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with { PartnerId = partnerResponse.Content!.Id };

        var borrowerResponse = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);
        await borrowerResponse.EnsureSuccessfulAsync();
        _borrower = borrowerResponse.Content!;
    }

    [Fact]
    public async Task GetBorrowerById_ShouldReturnBorrowerSuccessfully()
    {
        // Act
        var response = await _borrowersApi.GetBorrowerByIdAsync(_borrower.Id);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.Id.ShouldBe(_borrower.Id);
        response.Content.Surname.ShouldBe(_borrower.Surname);
        response.Content.GivenName.ShouldBe(_borrower.GivenName);
        response.Content.Gender.ShouldBe(_borrower.Gender);
        response.Content.DateOfBirth.ShouldBe(_borrower.DateOfBirth);

        _testOutputHelper.WriteLine("Retrieved Borrower ID: {0}", response.Content.Id);
    }

    [Fact]
    public async Task GetBorrowerById_ShouldReturnNotFoundForNonExistentBorrower()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _borrowersApi.GetBorrowerByIdAsync(nonExistentId);

        // Assert
        response.IsSuccessful.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        _testOutputHelper.WriteLine("Received expected NotFound for non-existent borrower ID: {0}", nonExistentId);
    }

    [Fact]
    public async Task GetBorrowerById_ShouldReturnNotFoundForEmptyGuid()
    {
        // Act
        var response = await _borrowersApi.GetBorrowerByIdAsync(Guid.Empty);

        // Assert
        response.IsSuccessful.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        _testOutputHelper.WriteLine("Received expected NotFound for empty GUID");
    }
}

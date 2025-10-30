using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

[Collection(BorrowersCollectionFixture.CollectionName)]
public class AddBorrowerTests : IAsyncLifetime
{
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private PartnerDTO _partner = default!;

    public AddBorrowerTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
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
        // Create a partner for testing borrowers
        var addPartnerCommand = BorrowerFakers.AddPartnerCommandFaker.Generate();
        var partnerResponse = await _borrowersApi.AddPartnerAsync(addPartnerCommand);
        await partnerResponse.EnsureSuccessfulAsync();
        _partner = partnerResponse.Content!;
    }

    [Fact]
    public async Task AddBorrower_ShouldCreateBorrowerSuccessfully()
    {
        // Arrange
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with { PartnerId = _partner.Id };

        // Act
        var response = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);
        var borrower = response.Content;

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        borrower.ShouldNotBeNull();
        borrower.Id.ShouldNotBe(Guid.Empty);
        borrower.Surname.ShouldBe(addBorrowerCommand.Surname);
        borrower.GivenName.ShouldBe(addBorrowerCommand.GivenName);
        borrower.Gender.ShouldBe(addBorrowerCommand.Gender);
        borrower.DateOfBirth.ShouldBe(addBorrowerCommand.DateOfBirth);
        borrower.PhoneNumber.ShouldBe(addBorrowerCommand.PhoneNumber);

        _testOutputHelper.WriteLine("Created Borrower ID: {0}", borrower.Id);
    }

    [Fact]
    public async Task AddBorrower_ShouldHandleYoungAdult()
    {
        // Arrange
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with 
        { 
            PartnerId = _partner.Id,
            DateOfBirth = DateOnly.FromDateTime(DateTime.Now.AddYears(-18)) // 18 years old
        };

        // Act
        var response = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.DateOfBirth.ShouldBe(addBorrowerCommand.DateOfBirth);

        _testOutputHelper.WriteLine("Created Young Adult Borrower ID: {0}", response.Content.Id);
    }

    [Fact]
    public async Task AddBorrower_ShouldHandleNonExistentPartner()
    {
        // Arrange
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with { PartnerId = Guid.NewGuid() }; // Non-existent partner

        // Act
        var response = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);

        // Assert
        response.IsSuccessful.ShouldBeFalse();

        _testOutputHelper.WriteLine("Correctly failed to create borrower with non-existent partner. Status: {0}", response.StatusCode);
    }
}

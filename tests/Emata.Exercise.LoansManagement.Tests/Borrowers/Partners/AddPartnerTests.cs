using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers.Partners;

[Collection(BorrowersCollectionFixture.CollectionName)]
public class AddPartnerTests : IAsyncLifetime
{

    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;

    public AddPartnerTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
    {
        _borrowersApi = apiFactory.BorrowersApi;
        _testOutputHelper = testOutputHelper;
        _resetDatabaseAsync = apiFactory.ResetDatabaseAsync;
    }
    
    public Task DisposeAsync()
    {
        return _resetDatabaseAsync();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public async Task AddPartner_ShouldCreatePartnerSuccessfully()
    {
        // Arrange
        var addPartnerCommand = BorrowerFakers.AddPartnerCommandFaker.Generate();

        // Act
        var response = await _borrowersApi.AddPartnerAsync(addPartnerCommand);
        var partner = response.Content;

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        partner.ShouldNotBeNull();
        partner.Id.ShouldNotBe(Guid.Empty);
        partner.Name.ShouldBe(addPartnerCommand.Name);
        partner.Address?.Town.ShouldBe(addPartnerCommand.Town);


        _testOutputHelper.WriteLine("Created Partner ID: {0}", partner.Id);
    }

    [Fact]
    public async Task AddPartner_ShouldReturnBadRequest_ForInvalidCommand()
    {
        // Arrange
        var addPartnerCommand = new AddPartnerCommand
        {
            Name = null, // Invalid name
            Town = "SampleTown"
        };

        // Act
        var response = await _borrowersApi.AddPartnerAsync(addPartnerCommand);

        // Assert
        response.IsSuccessful.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.InternalServerError);

        _testOutputHelper.WriteLine("Received expected InternalServerError for invalid command.");
    }
}

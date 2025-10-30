using System;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;
using Emata.Exercise.LoansManagement.Contracts.Loans;
using Emata.Exercise.LoansManagement.Contracts.Loans.DTOs;
using Emata.Exercise.LoansManagement.Tests.Borrowers;
using Emata.Exercise.LoansManagement.Tests.Setup;
using Shouldly;
using Xunit.Abstractions;

namespace Emata.Exercise.LoansManagement.Tests.Loans;

[Collection(LoansCollectionFixture.CollectionName)]
public class GetLoanByIdTests : IAsyncLifetime
{
    private readonly ILoansRefitApi _loansApi;
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private LoanItem _loan = default!;

    public GetLoanByIdTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
    {
        _loansApi = apiFactory.LoansApi;
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
        // Create a partner first
        var partner = BorrowerFakers.AddPartnerCommandFaker.Generate();
        var partnerResponse = await _borrowersApi.AddPartnerAsync(partner);
        await partnerResponse.EnsureSuccessfulAsync();

        // Create a borrower
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with { PartnerId = partnerResponse.Content!.Id };

        var borrowerResponse = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);
        await borrowerResponse.EnsureSuccessfulAsync();

        // Create a loan
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = borrowerResponse.Content!.Id;

        var loanResponse = await _loansApi.AddLoanAsync(addLoanCommand);
        await loanResponse.EnsureSuccessfulAsync();
        _loan = loanResponse.Content!;
    }

    [Fact]
    public async Task GetLoanById_ShouldReturnLoanSuccessfully()
    {
        // Act
        var response = await _loansApi.GetLoanByIdAsync(_loan.Id);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.Id.ShouldBe(_loan.Id);
        response.Content.BorrowerId.ShouldBe(_loan.BorrowerId);
        response.Content.LoanAmount.ShouldBe(_loan.LoanAmount);

        _testOutputHelper.WriteLine("Retrieved Loan ID: {0}", response.Content.Id);
    }

    [Fact]
    public async Task GetLoanById_ShouldReturnNotFoundForNonExistentLoan()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await _loansApi.GetLoanByIdAsync(nonExistentId);

        // Assert
        response.IsSuccessful.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        _testOutputHelper.WriteLine("Received expected NotFound for non-existent loan ID: {0}", nonExistentId);
    }

    [Fact]
    public async Task GetLoanById_ShouldReturnNotFoundForEmptyGuid()
    {
        // Act
        var response = await _loansApi.GetLoanByIdAsync(Guid.Empty);

        // Assert
        response.IsSuccessful.ShouldBeFalse();
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        _testOutputHelper.WriteLine("Received expected NotFound for empty GUID");
    }
}

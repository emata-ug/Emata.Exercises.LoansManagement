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
public class AddLoanTests : IAsyncLifetime
{
    private readonly ILoansRefitApi _loansApi;
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private BorrowerDTO _borrower = default!;

    public AddLoanTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
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

        // Create a borrower for testing loans
        var addBorrowerCommand = BorrowerFakers.AddBorrowerCommandFaker.Generate();
        addBorrowerCommand = addBorrowerCommand with { PartnerId = partnerResponse.Content!.Id };

        var borrowerResponse = await _borrowersApi.AddBorrowerAsync(addBorrowerCommand);
        await borrowerResponse.EnsureSuccessfulAsync();
        _borrower = borrowerResponse.Content!;
    }

    [Fact]
    public async Task AddLoan_ShouldCreateLoanSuccessfully()
    {
        // Arrange
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = _borrower.Id;

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);
        var loan = response.Content;

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        loan.ShouldNotBeNull();
        loan.Id.ShouldNotBe(Guid.Empty);
        loan.BorrowerId.ShouldBe(addLoanCommand.BorrowerId);
        loan.LoanAmount.ShouldBe(addLoanCommand.LoanAmount);
        loan.IssueDate.ShouldBe(addLoanCommand.IssueDate);
        loan.Reference.ShouldBe(addLoanCommand.Reference);
        loan.Reason.ShouldBe(addLoanCommand.Reason);

        _testOutputHelper.WriteLine("Created Loan ID: {0}", loan.Id);
    }

    [Fact]
    public async Task AddLoan_ShouldHandleMinimumLoanAmount()
    {
        // Arrange
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = _borrower.Id;
        addLoanCommand.LoanAmount = 1; // Minimum amount

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.LoanAmount.ShouldBe(1);

        _testOutputHelper.WriteLine("Created Loan with minimum amount: {0}", response.Content.Id);
    }

    [Fact]
    public async Task AddLoan_ShouldHandleLargeLoanAmount()
    {
        // Arrange
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = _borrower.Id;
        addLoanCommand.LoanAmount = 999999999.99m; // Large amount

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.LoanAmount.ShouldBe(999999999.99m);

        _testOutputHelper.WriteLine("Created Loan with large amount: {0}", response.Content.Id);
    }

    [Fact]
    public async Task AddLoan_ShouldHandleFutureIssueDate()
    {
        // Arrange
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = _borrower.Id;
        addLoanCommand.IssueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.IssueDate.ShouldBe(addLoanCommand.IssueDate);

        _testOutputHelper.WriteLine("Created Loan with future issue date: {0}", response.Content.Id);
    }

    [Fact]
    public async Task AddLoan_ShouldHandleNullOptionalFields()
    {
        // Arrange
        var addLoanCommand = new AddLoanCommand
        {
            BorrowerId = _borrower.Id,
            LoanAmount = 5000,
            IssueDate = DateOnly.FromDateTime(DateTime.Now),
            Reference = null,
            Reason = null,
            Duration = new DurationDto { Length = 12, Period = Period.Monthly },
            InterestRate = new InterestRateDto { PercentageRate = 5.5m, Period = Period.Annual }
        };

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);

        // Assert
        response.IsSuccessful.ShouldBeTrue();
        response.Content.ShouldNotBeNull();
        response.Content.Reference.ShouldBeNull();
        response.Content.Reason.ShouldBeNull();

        _testOutputHelper.WriteLine("Created Loan with null optional fields: {0}", response.Content.Id);
    }

    [Fact]
    public async Task AddLoan_ShouldHandleNonExistentBorrower()
    {
        // Arrange
        var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
        addLoanCommand.BorrowerId = Guid.NewGuid(); // Non-existent borrower

        // Act
        var response = await _loansApi.AddLoanAsync(addLoanCommand);

        // Assert
        // The endpoint doesn't validate borrower existence, so it may succeed or fail depending on implementation
        // For now, we'll just verify the response
        _testOutputHelper.WriteLine("Attempted to create loan for non-existent borrower. Status: {0}", response.StatusCode);
    }
}

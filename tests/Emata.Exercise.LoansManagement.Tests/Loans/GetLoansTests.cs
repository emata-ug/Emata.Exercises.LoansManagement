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
public class GetLoansTests : IAsyncLifetime
{
    private readonly ILoansRefitApi _loansApi;
    private readonly IBorrowersRefitApi _borrowersApi;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Func<Task> _resetDatabaseAsync;
    private List<LoanItem> _loans = [];
    private BorrowerDTO _borrower = default!;

    public GetLoansTests(ApiFactory apiFactory, ITestOutputHelper testOutputHelper)
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
        _borrower = borrowerResponse.Content!;

        // Create multiple loans
        var loanRequests = LoanFakers.AddLoanCommandFaker.Generate(new Random().Next(5, 10));
        foreach (var loanRequest in loanRequests)
        {
            loanRequest.BorrowerId = _borrower.Id;
            var response = await _loansApi.AddLoanAsync(loanRequest);
            await response.EnsureSuccessfulAsync();
            _loans.Add(response.Content!);
        }
    }

    [Fact]
    public async Task GetLoans_ShouldReturnAllLoans()
    {
        // Act
        var response = await _loansApi.GetLoansAsync(new GetLoansQuery());

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.Count.ShouldBeGreaterThanOrEqualTo(_loans.Count);

        _testOutputHelper.WriteLine("Retrieved {0} loans", response.Content.Count);
    }

    [Fact]
    public async Task GetLoans_ShouldFilterByBorrowerId()
    {
        // Act
        var query = new GetLoansQuery
        {
            BorrowerIds = [_borrower.Id]
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => loan.BorrowerId == _borrower.Id);
        response.Content.Count.ShouldBe(_loans.Count);

        _testOutputHelper.WriteLine("Retrieved {0} loans for borrower {1}", response.Content.Count, _borrower.Id);
    }

    [Fact]
    public async Task GetLoans_ShouldFilterByMinLoanAmount()
    {
        // Arrange
        var minAmount = _loans.OrderBy(l => l.LoanAmount).Skip(_loans.Count / 2).First().LoanAmount;

        // Act
        var query = new GetLoansQuery
        {
            MinLoanAmount = minAmount
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => loan.LoanAmount >= minAmount);

        _testOutputHelper.WriteLine("Retrieved {0} loans with minimum amount {1}", response.Content.Count, minAmount);
    }

    [Fact]
    public async Task GetLoans_ShouldFilterByMaxLoanAmount()
    {
        // Arrange
        var maxAmount = _loans.OrderBy(l => l.LoanAmount).Skip(_loans.Count / 2).First().LoanAmount;

        // Act
        var query = new GetLoansQuery
        {
            MaxLoanAmount = maxAmount
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => loan.LoanAmount <= maxAmount);

        _testOutputHelper.WriteLine("Retrieved {0} loans with maximum amount {1}", response.Content.Count, maxAmount);
    }

    [Fact]
    public async Task GetLoans_ShouldFilterByDateRange()
    {
        // Arrange
        var startDate = _loans.OrderBy(l => l.IssueDate).First().IssueDate;
        var endDate = _loans.OrderByDescending(l => l.IssueDate).First().IssueDate;

        // Act
        var query = new GetLoansQuery
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => loan.IssueDate >= startDate && loan.IssueDate <= endDate);

        _testOutputHelper.WriteLine("Retrieved {0} loans between {1} and {2}", response.Content.Count, startDate, endDate);
    }

    [Fact]
    public async Task GetLoans_ShouldReturnEmptyListForNonMatchingFilters()
    {
        // Act
        var query = new GetLoansQuery
        {
            BorrowerIds = [Guid.NewGuid()] // Non-existent borrower
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldBeEmpty();

        _testOutputHelper.WriteLine("Retrieved 0 loans for non-existent borrower");
    }

    [Fact]
    public async Task GetLoans_ShouldHandleMultipleFilters()
    {
        // Arrange
        var minAmount = _loans.Min(l => l.LoanAmount);
        var maxAmount = _loans.Max(l => l.LoanAmount);

        // Act
        var query = new GetLoansQuery
        {
            BorrowerIds = [_borrower.Id],
            MinLoanAmount = minAmount,
            MaxLoanAmount = maxAmount
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => 
            loan.BorrowerId == _borrower.Id && 
            loan.LoanAmount >= minAmount && 
            loan.LoanAmount <= maxAmount);

        _testOutputHelper.WriteLine("Retrieved {0} loans with multiple filters", response.Content.Count);
    }
}

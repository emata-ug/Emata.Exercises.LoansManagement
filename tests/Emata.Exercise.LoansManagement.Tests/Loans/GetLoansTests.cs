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
    private List<PartnerDTO> _partners = [];
    private List<BorrowerDTO> _borrowers = [];

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
        var random = new Random();
        
        // Create multiple partners
        var partnerRequests = BorrowerFakers.AddPartnerCommandFaker.Generate(random.Next(3, 5));
        foreach (var partnerRequest in partnerRequests)
        {
            var partnerResponse = await _borrowersApi.AddPartnerAsync(partnerRequest);
            await partnerResponse.EnsureSuccessfulAsync();
            _partners.Add(partnerResponse.Content!);
        }

        // Create multiple borrowers, randomly assigning partners
        var borrowerRequests = BorrowerFakers.AddBorrowerCommandFaker.Generate(random.Next(5, 8));
        foreach (var borrowerRequest in borrowerRequests)
        {
            var randomPartner = _partners[random.Next(_partners.Count)];
            var request = borrowerRequest with { PartnerId = randomPartner.Id };
            var borrowerResponse = await _borrowersApi.AddBorrowerAsync(request);
            await borrowerResponse.EnsureSuccessfulAsync();
            _borrowers.Add(borrowerResponse.Content!);
        }

        // Create multiple loans, randomly assigning borrowers
        var loanRequests = LoanFakers.AddLoanCommandFaker.Generate(random.Next(5, 10));
        foreach (var loanRequest in loanRequests)
        {
            var randomBorrower = _borrowers[random.Next(_borrowers.Count)];
            loanRequest.BorrowerId = randomBorrower.Id;
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
        // Arrange
        var testBorrower = _borrowers.First();
        var expectedLoansCount = _loans.Count(l => l.BorrowerId == testBorrower.Id);
        
        // Act
        var query = new GetLoansQuery
        {
            BorrowerIds = [testBorrower.Id]
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => loan.BorrowerId == testBorrower.Id);
        response.Content.Count.ShouldBe(expectedLoansCount);

        _testOutputHelper.WriteLine("Retrieved {0} loans for borrower {1}", response.Content.Count, testBorrower.Id);
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
        var testBorrower = _borrowers.First();
        var minAmount = _loans.Min(l => l.LoanAmount);
        var maxAmount = _loans.Max(l => l.LoanAmount);

        // Act
        var query = new GetLoansQuery
        {
            BorrowerIds = [testBorrower.Id],
            MinLoanAmount = minAmount,
            MaxLoanAmount = maxAmount
        };
        var response = await _loansApi.GetLoansAsync(query);

        // Assert
        await response.EnsureSuccessfulAsync();
        response.Content.ShouldNotBeNull();
        response.Content.ShouldAllBe(loan => 
            loan.BorrowerId == testBorrower.Id && 
            loan.LoanAmount >= minAmount && 
            loan.LoanAmount <= maxAmount);

        _testOutputHelper.WriteLine("Retrieved {0} loans with multiple filters", response.Content.Count);
    }
}

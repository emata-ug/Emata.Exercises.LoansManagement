# Testing Guide

This document provides comprehensive information about testing the Loans Management System.

## Table of Contents

- [Testing Requirements](#testing-requirements)
- [Running Tests](#running-tests)
- [Testing Tools & Libraries](#testing-tools--libraries)
- [Test Structure](#test-structure)
- [Test Coverage](#test-coverage)

## Testing Requirements

### Prerequisites

- **.NET 9.0 SDK** - Required for building and running tests
- **Docker** - Required for running PostgreSQL test containers
- **~2GB RAM** - For Docker containers during test execution

### System Requirements

The integration tests use Testcontainers to spin up a PostgreSQL database in Docker. Ensure:
1. Docker is installed and running
2. Docker daemon is accessible
3. Sufficient disk space for container images (~500MB)

## Running Tests

### Run All Tests

```bash
# From the solution root directory
dotnet test

# With verbose output
dotnet test --verbosity normal

# With detailed output
dotnet test --verbosity detailed
```

### Run Specific Test Classes

```bash
# Run only Borrower tests
dotnet test --filter "FullyQualifiedName~Borrowers"

# Run only Loan tests
dotnet test --filter "FullyQualifiedName~Loans"

# Run only Partner tests
dotnet test --filter "FullyQualifiedName~Partners"
```

### Run Specific Tests

```bash
# Run a specific test method
dotnet test --filter "FullyQualifiedName~AddBorrowerTests.AddBorrower_ShouldCreateBorrowerSuccessfully"
```

### Generate Code Coverage

```bash
# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Coverage reports will be generated in: TestResults/*/coverage.cobertura.xml
```

## Testing Tools & Libraries

### Core Testing Framework

#### xUnit (v2.9.2)
- **Purpose**: Primary testing framework
- **Functionality**:
  - `[Fact]` - Marks methods as test methods
  - `[Collection]` - Groups tests that share the same context
  - `IAsyncLifetime` - Provides setup/teardown for async operations
- **Usage Example**:
  ```csharp
  [Fact]
  public async Task TestName()
  {
      // Arrange, Act, Assert
  }
  ```

### Assertion Library

#### Shouldly (v4.2.1)
- **Purpose**: Fluent assertion library for readable test assertions
- **Functionality**:
  - `ShouldBe()` - Asserts equality
  - `ShouldBeTrue()` / `ShouldBeFalse()` - Boolean assertions
  - `ShouldNotBeNull()` - Null checks
  - `ShouldBeGreaterThan()` / `ShouldBeLessThan()` - Comparison assertions
  - `ShouldAllBe()` - Collection assertions
- **Usage Example**:
  ```csharp
  borrower.Id.ShouldNotBe(Guid.Empty);
  response.IsSuccessful.ShouldBeTrue();
  borrowers.Count.ShouldBeGreaterThanOrEqualTo(5);
  ```

### Test Infrastructure

#### Testcontainers (v3.10.0)
- **Purpose**: Manages Docker containers for integration testing
- **Functionality**:
  - Automatically starts PostgreSQL container before tests
  - Provides isolated database for each test run
  - Cleans up containers after test execution
- **Configuration**: See `ApiFactory.cs` for container setup

#### ASP.NET Core Testing (WebApplicationFactory)
- **Purpose**: In-memory testing of ASP.NET Core applications
- **Functionality**:
  - Starts the web application in-memory
  - No need for external server process
  - Full HTTP client for making requests

#### Refit (v7.2.22)
- **Purpose**: Type-safe REST API client
- **Functionality**:
  - Converts interface definitions to HTTP API calls
  - Automatic serialization/deserialization
  - Built-in HTTP response handling
- **Usage Example**:
  ```csharp
  public interface ILoansRefitApi
  {
      [Post("/loans")]
      Task<ApiResponse<LoanItem>> AddLoanAsync([Body] AddLoanCommand loan);
  }
  ```

#### Respawn (v6.2.1)
- **Purpose**: Database cleanup between tests
- **Functionality**:
  - Efficiently resets database to clean state
  - Preserves schema and migrations
  - Removes all data without dropping/recreating database
- **Benefits**: Faster test execution compared to database recreation

### Test Data Generation

#### Bogus (v35.6.1)
- **Purpose**: Generates realistic fake data for testing
- **Functionality**:
  - Creates random names, addresses, phone numbers, etc.
  - Customizable data generation rules
  - Consistent across test runs when using seeds
- **Usage Example**:
  ```csharp
  public static Faker<AddBorrowerCommand> AddBorrowerCommandFaker => 
      new Faker<AddBorrowerCommand>()
          .CustomInstantiator(faker => new AddBorrowerCommand
          {
              Surname = faker.Name.LastName(),
              GivenName = faker.Name.FirstName(),
              Gender = faker.PickRandom<Gender>(),
              PhoneNumber = faker.Phone.PhoneNumber()
          });
  ```

## Test Structure

### Test Organization

Tests are organized by module following the module structure:

```
tests/
└── Emata.Exercise.LoansManagement.Tests/
    ├── Borrowers/
    │   ├── [Test classes per endpoint]
    │   ├── BorrowerFakers.cs (Test data generators)
    │   ├── IBorrowersRefitApi.cs (Refit API interface)
    │   ├── BorrowersCollectionFixture.cs (xUnit collection fixture)
    │   └── Partners/ (Sub-module for Partners endpoints)
    ├── Loans/
    │   ├── [Test classes per endpoint]
    │   ├── LoanFakers.cs (Test data generators)
    │   ├── ILoansRefitApi.cs (Refit API interface)
    │   └── LoansCollectionFixture.cs (xUnit collection fixture)
    └── Setup/
        └── ApiFactory.cs (Shared test infrastructure)
```

### Test Naming Convention

Tests follow the pattern: `MethodName_Should_ExpectedBehavior_When_Condition` (when applicable)

Examples:
- `AddBorrower_ShouldCreateBorrowerSuccessfully`
- `GetLoanById_ShouldReturnNotFound_WhenLoanDoesNotExist`
- `GetLoans_ShouldFilterByBorrowerId`
- `GetBorrowerById_ShouldReturnNotFound_WhenIdIsEmpty`

### Test Pattern (AAA)

All tests follow the Arrange-Act-Assert pattern:

```csharp
[Fact]
public async Task AddLoan_ShouldCreateLoanSuccessfully()
{
    // Arrange - Set up test data
    var addLoanCommand = LoanFakers.AddLoanCommandFaker.Generate();
    addLoanCommand.BorrowerId = _borrower.Id;

    // Act - Execute the operation
    var response = await _loansApi.AddLoanAsync(addLoanCommand);

    // Assert - Verify the results
    response.IsSuccessful.ShouldBeTrue();
    response.Content.ShouldNotBeNull();
    response.Content.LoanAmount.ShouldBe(addLoanCommand.LoanAmount);
}
```

## Test Coverage

### Test Statistics

- **Total Tests**: 38
- **Modules Covered**: 2 (Borrowers, Loans)
- **Endpoints Tested**: 7
- **Coverage Areas**: CRUD operations, filtering, edge cases, error scenarios

## Best Practices

1. **Isolation**: Each test is isolated and doesn't depend on other tests
2. **Cleanup**: Database is reset between tests using Respawn
3. **Realistic Data**: Use Bogus fakers for generating test data
4. **Edge Cases**: Include tests for boundary conditions and error scenarios
5. **Descriptive Names**: Test names clearly describe what is being tested
6. **Async/Await**: All async operations are properly awaited

## Troubleshooting

### Docker Container Issues

If tests fail to start:
```bash
# Check Docker is running
docker ps

# Clean up old containers
docker system prune -f
```

### Database Connection Issues

If database connection fails:
```bash
# Check PostgreSQL container logs
docker logs <container-id>

# Ensure port 5432 is not in use
netstat -an | grep 5432
```

### Test Timeout Issues

If tests timeout:
- Increase the timeout in test settings
- Check system resources (RAM, CPU)
- Ensure Docker has enough resources allocated

## Additional Resources

- [xUnit Documentation](https://xunit.net/)
- [Shouldly Documentation](https://github.com/shouldly/shouldly)
- [Testcontainers Documentation](https://dotnet.testcontainers.org/)
- [Bogus Documentation](https://github.com/bchavez/Bogus)
- [Refit Documentation](https://github.com/reactiveui/refit)

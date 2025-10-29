using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers.Domain;

public class BorrowerBuilderTests
{
    [Fact]
    public void Build_WithAllRequiredFields_Succeeds()
    {
        // Arrange & Act
        var partnerId = Guid.NewGuid();
        var borrower = BorrowerBuilder.Create()
            .SetSurname("Doe")
            .SetGivenName("John")
            .SetGender(Gender.Male)
            .SetDateOfBirth(new DateOnly(1990, 1, 1))
            .SetPhoneNumber("+123456789")
            .SetPartnerId(partnerId)
            .SetTown("Kampala")
            .SetEmail("john.doe@example.com")
            .Build();

        // Assert
        Assert.Equal("Doe", borrower.Surname);
        Assert.Equal("John", borrower.GivenName);
        Assert.Equal("John Doe", borrower.Name);
        Assert.Equal(new DateOnly(1990, 1, 1), borrower.DateOfBirth);
        Assert.Equal("+123456789", borrower.PhoneNumber);
        Assert.Equal(partnerId, borrower.PartnerId);
        Assert.NotNull(borrower.Address);
        Assert.Equal("Kampala", borrower.Address!.Town);
        Assert.Equal("john.doe@example.com", borrower.Email);
        Assert.True(borrower.CreatedOn <= DateTime.UtcNow.AddSeconds(1));
    }

    [Fact]
    public void Build_MissingRequiredFields_Throws()
    {
        var builder = BorrowerBuilder.Create();
        var ex = Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Contains("Surname", ex.Message);
        Assert.Contains("GivenName", ex.Message);
        Assert.Contains("DateOfBirth", ex.Message);
        Assert.Contains("PhoneNumber", ex.Message);
        Assert.Contains("PartnerId", ex.Message);
    }
}

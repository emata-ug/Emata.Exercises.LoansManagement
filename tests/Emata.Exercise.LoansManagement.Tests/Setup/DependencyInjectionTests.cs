using Emata.Exercise.LoansManagement.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shouldly;

namespace Emata.Exercises.LoanManagement.IntegrationTests;

[Collection(DefaultCollectionFixture.Name)]
public class DependencyInjectionTests
{
    private readonly ApiFactory _apiFactory;
    public DependencyInjectionTests(ApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }
    [Fact]
    public void DependencyInjection_ShouldBuildSuccessfully()
    {
        // Arrange & Act
        var host = _apiFactory.Services.GetRequiredService<IHost>();

        // Assert
        host.ShouldNotBeNull();
    }
}

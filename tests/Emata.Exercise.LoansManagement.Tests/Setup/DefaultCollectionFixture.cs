namespace Emata.Exercise.LoansManagement.Tests.Setup;

[CollectionDefinition(Name)]
public class DefaultCollectionFixture : ICollectionFixture<ApiFactory>
{
   public const string Name = "Default integration tests";
}
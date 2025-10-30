using System;
using Emata.Exercise.LoansManagement.Tests.Setup;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

[CollectionDefinition(CollectionName)]
public class BorrowersCollectionFixture : ICollectionFixture<ApiFactory>
{
    public const string CollectionName = "BorrowersCollection";
}

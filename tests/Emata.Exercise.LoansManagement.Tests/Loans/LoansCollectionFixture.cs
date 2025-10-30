using System;
using Emata.Exercise.LoansManagement.Tests.Setup;

namespace Emata.Exercise.LoansManagement.Tests.Loans;

[CollectionDefinition(CollectionName)]
public class LoansCollectionFixture : ICollectionFixture<ApiFactory>
{
    public const string CollectionName = "LoansCollection";
}

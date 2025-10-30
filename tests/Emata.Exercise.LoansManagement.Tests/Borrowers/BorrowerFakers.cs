using System;
using Bogus;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Tests.Borrowers;

public static class BorrowerFakers
{
    public static Faker<AddPartnerCommand> AddPartnerCommandFaker => new Faker<AddPartnerCommand>()
        .CustomInstantiator(faker => new AddPartnerCommand()
        {
            Name = faker.Company.CompanyName(),
            Town = faker.Address.City()
        });
}

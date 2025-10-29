using System;
using Emata.Exercise.LoansManagement.Borrowers.Domain;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

namespace Emata.Exercise.LoansManagement.Borrowers.UseCases;

internal static class MappingExtensions
{
    public static PartnerDTO ToDTO(this Partner partner)
    {
        ArgumentNullException.ThrowIfNull(partner);
        return new PartnerDTO
        {
            Id = partner.Id,
            Name = partner.Name,
            Address = partner.Address is not null
                ? new AddressDTO
                {
                    Town = partner.Address.Town,
                    District = partner.Address.District,
                    Region = partner.Address.Region,
                    Country = partner.Address.Country
                }
                : null
        };
    }

    public static AddressDTO ToDTO(this Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        return new AddressDTO
        {
            Town = address.Town,
            District = address.District,
            Region = address.Region,
            Country = address.Country
        };
    }

    public static BorrowerSummaryDTO ToSummaryDTO(this Borrower borrower)
    {
        ArgumentNullException.ThrowIfNull(borrower);
        return new BorrowerSummaryDTO
        {
            Id = borrower.Id,
            Name = borrower.Name,
            PartnerName = borrower.Partner?.Name,
            PhoneNumber = borrower.PhoneNumber,
            Town = borrower.Address?.Town
        };
    }

    public static BorrowerDTO ToDTO(this Borrower borrower)
    {
        ArgumentNullException.ThrowIfNull(borrower);
        return new BorrowerDTO
        {
            Id = borrower.Id,
            Surname = borrower.Surname,
            GivenName = borrower.GivenName,
            Name = borrower.Name,
            DateOfBirth = borrower.DateOfBirth,
            IdentificationNumber = borrower.IdentificationNumber,
            PhoneNumber = borrower.PhoneNumber,
            Email = borrower.Email,
            CreatedOn = borrower.CreatedOn,
            AddressDTO = borrower.Address is not null
                ? new AddressDTO
                {
                    Town = borrower.Address.Town,
                    District = borrower.Address.District,
                    Region = borrower.Address.Region,
                    Country = borrower.Address.Country
                }
                : null,
            PartnerDTO = borrower.Partner is not null
                ? borrower.Partner.ToDTO()
                : null
        };
    }
}

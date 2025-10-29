using System.Runtime.CompilerServices;
using Emata.Exercise.LoansManagement.Contracts.Borrowers.DTOs;

[assembly: InternalsVisibleTo("Emata.Exercise.LoansManagement.Tests")]

namespace Emata.Exercise.LoansManagement.Borrowers.Domain;

/// <summary>
/// Fluent builder for constructing <see cref="Borrower"/> instances while enforcing required fields.
/// </summary>
internal sealed class BorrowerBuilder
{
    private string? _surname;
    private string? _givenName;
    private Gender? _gender;
    private DateOnly? _dateOfBirth;
    private string? _phoneNumber;
    private DateTime? _createdOn;
    private string? _town;
    private Guid? _partnerId;
    private string? _identificationNumber;
    private string? _email;

    /// <summary>
    /// Start a new builder instance.
    /// </summary>
    public static BorrowerBuilder Create() => new();

    /// <summary>
    /// Convenience for starting a builder with a partner id.
    /// </summary>
    public static BorrowerBuilder ForPartner(Guid partnerId) => new BorrowerBuilder().SetPartnerId(partnerId);

    public BorrowerBuilder SetSurname(string surname)
    {
        _surname = surname?.Trim();
        return this;
    }

    public BorrowerBuilder SetGivenName(string givenName)
    {
        _givenName = givenName?.Trim();
        return this;
    }

    public BorrowerBuilder SetGender(Gender gender)
    {
        _gender = gender;
        return this;
    }

    public BorrowerBuilder SetDateOfBirth(DateOnly dateOfBirth)
    {
        _dateOfBirth = dateOfBirth;
        return this;
    }

    public BorrowerBuilder SetPhoneNumber(string phoneNumber)
    {
        _phoneNumber = phoneNumber?.Trim();
        return this;
    }

    public BorrowerBuilder SetCreatedOn(DateTime createdOn)
    {
        _createdOn = createdOn;
        return this;
    }

    public BorrowerBuilder SetTown(string? town)
    {
        _town = string.IsNullOrWhiteSpace(town) ? null : town.Trim();
        return this;
    }

    public BorrowerBuilder SetPartnerId(Guid partnerId)
    {
        _partnerId = partnerId;
        return this;
    }

    public BorrowerBuilder SetIdentificationNumber(string? identificationNumber)
    {
        _identificationNumber = string.IsNullOrWhiteSpace(identificationNumber) ? null : identificationNumber.Trim();
        return this;
    }

    public BorrowerBuilder SetEmail(string? email)
    {
        _email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        return this;
    }

    /// <summary>
    /// Build the <see cref="Borrower"/>. Throws <see cref="InvalidOperationException"/> if any required fields are missing.
    /// </summary>
    public Borrower Build()
    {
        var missing = GetMissingRequiredFields();
        if (missing.Count > 0)
        {
            throw new InvalidOperationException($"Cannot build Borrower. Missing required fields: {string.Join(", ", missing)}");
        }

        // Use UTC now if caller did not provide CreatedOn
        var createdOn = _createdOn ?? DateTime.UtcNow;

        return Borrower.Create(
            surname: _surname!,
            givenName: _givenName!,
            gender: _gender!.Value,
            dateOfBirth: _dateOfBirth!.Value,
            phoneNumber: _phoneNumber!,
            createdOn: createdOn,
            town: _town,
            partnerId: _partnerId!.Value,
            identificationNumber: _identificationNumber,
            email: _email);
    }

    private List<string> GetMissingRequiredFields()
    {
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(_surname)) missing.Add(nameof(Borrower.Surname));
        if (string.IsNullOrWhiteSpace(_givenName)) missing.Add(nameof(Borrower.GivenName));
        if (_dateOfBirth is null) missing.Add(nameof(Borrower.DateOfBirth));
        if (_gender is null) missing.Add(nameof(Borrower.Gender));
        if (string.IsNullOrWhiteSpace(_phoneNumber)) missing.Add(nameof(Borrower.PhoneNumber));
        if (_partnerId is null) missing.Add(nameof(Borrower.PartnerId));
        return missing;
    }
}

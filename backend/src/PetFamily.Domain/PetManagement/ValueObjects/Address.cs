using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record Address
{
    public const int MAX_TEXT_LENGHT = 100;

    private Address(string country, string city, string street, string house, int? block)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Block = block;
    }

    public string Country { get; }

    public string City { get; }

    public string Street { get; }

    public string House { get; }

    public int? Block { get; }

    public static Result<Address, Error> Create(string country, string city, string street, string house, int? block)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > MAX_TEXT_LENGHT)
        {
            return Errors.General.ValueIsInvalid(nameof(Country));
        }

        if (string.IsNullOrWhiteSpace(city) || city.Length > MAX_TEXT_LENGHT)
        {
            return Errors.General.ValueIsInvalid(nameof(City));
        }

        if (string.IsNullOrWhiteSpace(street) || street.Length > MAX_TEXT_LENGHT)
        {
            return Errors.General.ValueIsInvalid(nameof(Street));
        }

        if (string.IsNullOrWhiteSpace(house) || house.Length > MAX_TEXT_LENGHT)
        {
            return Errors.General.ValueIsInvalid(nameof(House));
        }

        return new Address(country, city, street, house, block);
    }
}
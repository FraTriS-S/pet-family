using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record Address
{
    private Address(string country, string city, string street, string house, string? block)
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

    public string? Block { get; }

    public static Result<Address> Create(string country, string city, string street, string house, string? block)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return Result.Failure<Address>("Country is required");
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Failure<Address>("City is required");
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            return Result.Failure<Address>("Street is required");
        }

        if (string.IsNullOrWhiteSpace(house))
        {
            return Result.Failure<Address>("House is required");
        }

        return new Address(country, city, street, house, block);
    }
}
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer.Pet.ValueObjects;

public record Address
{
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

    public static Result<Address> Create(string country, string city, string street, string house, int? block)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return "Country is required";
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return "City is required";
        }

        if (string.IsNullOrWhiteSpace(street))
        {
            return "Street is required";
        }

        if (string.IsNullOrWhiteSpace(house))
        {
            return "House is required";
        }

        return new Address(country, city, street, house, block);
    }
}
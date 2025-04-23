using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.PetManagement.ValueObjects;

public record SocialNetwork
{
    [JsonConstructor]
    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }

    public string Url { get; }

    public static Result<SocialNetwork, Error> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Errors.General.ValueIsInvalid(nameof(Name));
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            return Errors.General.ValueIsInvalid(nameof(Url));
        }

        return new SocialNetwork(name, url);
    }
}
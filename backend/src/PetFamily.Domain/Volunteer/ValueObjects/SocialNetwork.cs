using PetFamily.Domain.Shared;

namespace PetFamily.Domain.Volunteer.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork(string name, string url)
    {
        Name = name;
        Url = url;
    }

    public string Name { get; }

    public string Url { get; }

    public static Result<SocialNetwork> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Name is required";
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            return "URL is required";
        }

        return new SocialNetwork(name, url);
    }
}
﻿using CSharpFunctionalExtensions;

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
            return Result.Failure<SocialNetwork>("Name is required");
        }

        if (string.IsNullOrWhiteSpace(url))
        {
            return Result.Failure<SocialNetwork>("URL is required");
        }

        return new SocialNetwork(name, url);
    }
}
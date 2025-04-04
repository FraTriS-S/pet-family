namespace PetFamily.Domain.Shared.ValueObjects;

public record PaymentDetails
{
    private PaymentDetails(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; }

    public string Description { get; }

    public static Result<PaymentDetails> Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "Name is required";
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            return "Description is required";
        }

        return new PaymentDetails(name, description);
    }
}
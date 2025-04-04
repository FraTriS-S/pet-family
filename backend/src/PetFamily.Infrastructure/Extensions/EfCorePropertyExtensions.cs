using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions;

public static class EfCorePropertyExtensions
{
    public static void JsonValueObjectCollectionConversion<TValueObject>(this PropertyBuilder<IReadOnlyList<TValueObject>> builder)
    {
        builder.HasConversion(
            v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
            json => JsonSerializer.Deserialize<IReadOnlyList<TValueObject>>(json, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<TValueObject>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }
}
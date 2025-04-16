using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Extensions;

public static class EfCorePropertyExtensions
{
    public static void JsonValueObjectCollectionConversion<TValueObject>(this PropertyBuilder<IReadOnlyList<TValueObject>> builder)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), // Разрешает русские символы
        };

        builder.HasConversion(
            v => JsonSerializer.Serialize(v, options),
            json => JsonSerializer.Deserialize<IReadOnlyList<TValueObject>>(json, options)!,
            new ValueComparer<IReadOnlyList<TValueObject>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                c => c.ToList()));
    }
}
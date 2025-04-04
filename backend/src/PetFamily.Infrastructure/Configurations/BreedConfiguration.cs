using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species.Breed;
using PetFamily.Domain.Species.Breed.ValueObjects;

namespace PetFamily.Infrastructure.Configurations;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value)
            )
            .IsRequired()
            .HasColumnName("id");

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("name");
    }
}
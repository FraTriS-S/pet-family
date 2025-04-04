using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Species;
using PetFamily.Domain.Species.ValueObjects;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value)
            )
            .IsRequired()
            .HasColumnName("id");

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("name");
    }
}
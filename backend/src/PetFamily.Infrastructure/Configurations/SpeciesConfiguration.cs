using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.Ids;
using PetFamily.Domain.Species.AggregateRoot;
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

        builder.ComplexProperty(v => v.Name, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(SpeciesName.MAX_LENGTH)
                .HasColumnName("name");
        });

        builder.HasMany(v => v.Breeds)
            .WithOne()
            .HasForeignKey("species_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteer.Pet;
using PetFamily.Domain.Volunteer.Pet.ValueObjects;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value)
            )
            .IsRequired()
            .HasColumnName("id");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("name");

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT)
            .HasColumnName("description");

        builder.Property(p => p.Gender)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("gender");

        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
            .HasColumnName("color");

        builder.ComplexProperty(p => p.SpeciesId, sb =>
        {
            sb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("species_id");
        });

        builder.ComplexProperty(p => p.BreedId, bb =>
        {
            bb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("breed_id");
        });

        builder.Property(p => p.Height)
            .IsRequired()
            .HasColumnName("height");

        builder.Property(p => p.Weight)
            .IsRequired()
            .HasColumnName("weight");

        builder.Property(p => p.HealthInfo)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGHT)
            .HasColumnName("health_info");

        builder.Property(p => p.HelpStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("help_status");

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("country");

            ab.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("city");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("street");

            ab.Property(a => a.House)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("house");

            ab.Property(a => a.Block)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGHT)
                .HasColumnName("block");
        });

        builder.Property(p => p.BirthDate)
            .IsRequired()
            .HasColumnName("birth_date");

        builder.Property(p => p.IsNeutered)
            .IsRequired()
            .HasColumnName("is_neutered");

        builder.Property(p => p.IsVaccinated)
            .IsRequired()
            .HasColumnName("is_vaccinated");

        builder.Property(v => v.PaymentDetails)
            .JsonValueObjectCollectionConversion();

        builder.Property(p => p.CreatedDate)
            .IsRequired()
            .HasColumnName("created_date");

        builder.ComplexProperty(p => p.VolunteerPhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("volunteer_phone_number");
        });
    }
}
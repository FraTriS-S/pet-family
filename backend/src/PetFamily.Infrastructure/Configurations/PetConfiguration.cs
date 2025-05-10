using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared.Ids;
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

        builder.ComplexProperty(v => v.Name, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(PetName.MAX_LENGTH)
                .HasColumnName("name");
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(p => p.Value)
                .IsRequired(false)
                .HasMaxLength(Description.MAX_LENGTH)
                .HasColumnName("description");
        });

        builder.ComplexProperty(v => v.Gender, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnName("gender");
        });

        builder.ComplexProperty(v => v.Color, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Color.MAX_LENGTH)
                .HasColumnName("color");
        });

        builder.ComplexProperty(p => p.SpeciesId, sb =>
        {
            sb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("species_id");
        });

        builder.Property(p => p.BreedId)
            .IsRequired()
            .HasColumnName("breed_id");

        builder.ComplexProperty(p => p.Height, bb =>
        {
            bb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("height");
        });

        builder.ComplexProperty(p => p.Weight, bb =>
        {
            bb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("weight");
        });

        builder.ComplexProperty(v => v.HealthInfo, db =>
        {
            db.Property(p => p.Value)
                .IsRequired(false)
                .HasMaxLength(HealthInfo.MAX_LENGTH)
                .HasColumnName("health_info");
        });

        builder.ComplexProperty(v => v.HelpStatus, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnName("help_status");
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Country)
                .IsRequired()
                .HasMaxLength(Address.MAX_TEXT_LENGHT)
                .HasColumnName("country");

            ab.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(Address.MAX_TEXT_LENGHT)
                .HasColumnName("city");

            ab.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(Address.MAX_TEXT_LENGHT)
                .HasColumnName("street");

            ab.Property(a => a.House)
                .IsRequired()
                .HasMaxLength(Address.MAX_TEXT_LENGHT)
                .HasColumnName("house");

            ab.Property(a => a.Block)
                .IsRequired(false)
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

        builder.ComplexProperty(p => p.Position, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("serial_number");
        });

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteer;
using PetFamily.Domain.Volunteer.ValueObjects;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value)
            )
            .IsRequired()
            .HasColumnName("id");

        builder.ComplexProperty(v => v.FullName, fnb =>
        {
            fnb.Property(fn => fn.FirstName)
                .IsRequired()
                .HasMaxLength(FullName.MAX_LENGTH)
                .HasColumnName("first_name");

            fnb.Property(fn => fn.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasMaxLength(FullName.MAX_LENGTH);

            fnb.Property(fn => fn.MiddleName)
                .HasColumnName("middle_name")
                .IsRequired()
                .HasMaxLength(FullName.MAX_LENGTH);
        });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(p => p.Value)
                .IsRequired(false)
                .HasMaxLength(Description.MAX_LENGTH)
                .HasColumnName("description");
        });

        builder.Property(v => v.Gender)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnName("gender");

        builder.ComplexProperty(v => v.PhoneNumber, pb =>
        {
            pb.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("phone_number");
        });

        builder.ComplexProperty(v => v.Email, eb =>
        {
            eb.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("email");
        });

        builder.ComplexProperty(v => v.Experience, db =>
        {
            db.Property(p => p.Value)
                .IsRequired()
                .HasColumnName("experience");
        });

        builder.Property(v => v.SocialNetworks)
            .JsonValueObjectCollectionConversion();

        builder.Property(v => v.PaymentDetails)
            .JsonValueObjectCollectionConversion();

        builder.HasMany(v => v.Pets)
            .WithOne(p => p.Volunteer)
            .HasForeignKey("volunteer_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Navigation(v => v.Pets).AutoInclude();
    }
}
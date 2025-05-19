using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.Add;

public class AddPetCommandValidator : AbstractValidator<AddPetCommand>
{
    public AddPetCommandValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(PetName.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Gender).MustBeValueObject(Gender.Create);
        RuleFor(u => u.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.Color).MustBeValueObject(Color.Create);
        RuleFor(c => c.Weight).MustBeValueObject(Weight.Create);
        RuleFor(c => c.Height).MustBeValueObject(Height.Create);
        RuleFor(c => c.HealthInfo).MustBeValueObject(HealthInfo.Create);
        RuleFor(c => c.HelpStatus).MustBeValueObject(HelpStatus.Create);
        RuleFor(c => c.Address).MustBeValueObject(x =>
            Address.Create(x.Country, x.City, x.Street, x.House, x.Block));
        RuleFor(u => u.BirthDate).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IsNeutered).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.IsVaccinated).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.VolunteerPhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}
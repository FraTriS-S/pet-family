using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateMainInfo;

public class UpdateVolunteerMainInfoCommandValidator : AbstractValidator<UpdateVolunteerMainInfoCommand>
{
    public UpdateVolunteerMainInfoCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.FullName).MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName, x.MiddleName));
        RuleFor(u => u.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Gender).MustBeValueObject(Gender.Create);
        RuleFor(u => u.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);
        RuleFor(u => u.Experience).MustBeValueObject(Experience.Create);
    }
}
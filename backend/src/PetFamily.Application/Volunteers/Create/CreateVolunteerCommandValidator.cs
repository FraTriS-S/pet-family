using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => new { c.FirstName, c.LastName, c.MiddleName })
            .MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName, x.MiddleName));
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Gender).MustBeValueObject(Gender.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleFor(c => c.Email).MustBeValueObject(Email.Create);

        RuleForEach(c => c.PaymentDetails)
            .MustBeValueObject(x => PaymentDetails.Create(x.Name, x.Description));
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Url));
    }
}
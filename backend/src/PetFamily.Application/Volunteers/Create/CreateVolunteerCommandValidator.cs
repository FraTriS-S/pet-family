using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.FullName).MustBeValueObject(x => FullName.Create(x.FirstName, x.LastName, x.MiddleName));
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
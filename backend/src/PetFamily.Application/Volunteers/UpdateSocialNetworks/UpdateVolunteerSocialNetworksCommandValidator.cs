using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdateSocialNetworks;

public class UpdateVolunteerSocialNetworksCommandValidator : AbstractValidator<UpdateVolunteerSocialNetworksCommand>
{
    public UpdateVolunteerSocialNetworksCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(x => SocialNetwork.Create(x.Name, x.Url));
    }
}
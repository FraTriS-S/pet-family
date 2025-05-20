using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.Pets.RemovePhotos;

public class RemovePetPhotosCommandValidator : AbstractValidator<RemovePetPhotosCommand>
{
    public RemovePetPhotosCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleForEach(c => c.PhotoNames).MustBeValueObject(FilePath.Create);
    }
}
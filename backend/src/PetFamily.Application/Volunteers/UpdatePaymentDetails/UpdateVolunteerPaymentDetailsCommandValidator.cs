using FluentValidation;
using PetFamily.Application.Validation;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.Volunteers.UpdatePaymentDetails;

public class UpdateVolunteerPaymentDetailsCommandValidator : AbstractValidator<UpdateVolunteerPaymentDetailsCommand>
{
    public UpdateVolunteerPaymentDetailsCommandValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleForEach(c => c.PaymentDetails)
            .MustBeValueObject(x => PaymentDetails.Create(x.Name, x.Description));
    }
}
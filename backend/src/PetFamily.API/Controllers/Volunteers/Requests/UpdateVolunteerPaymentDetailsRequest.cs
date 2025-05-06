using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.Volunteers.UpdatePaymentDetails;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record UpdateVolunteerPaymentDetailsRequest(
    Guid VolunteerId,
    IEnumerable<PaymentDetailsDto> PaymentDetails)
{
    public UpdateVolunteerPaymentDetailsCommand ToCommand() =>
        new(VolunteerId, PaymentDetails);
}
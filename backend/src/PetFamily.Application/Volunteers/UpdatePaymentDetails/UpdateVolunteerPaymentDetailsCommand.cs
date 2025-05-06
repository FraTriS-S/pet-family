using PetFamily.Application.DTOs.Shared;

namespace PetFamily.Application.Volunteers.UpdatePaymentDetails;

public record UpdateVolunteerPaymentDetailsCommand(
    Guid VolunteerId,
    IEnumerable<PaymentDetailsDto> PaymentDetails);
using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.ValueObjects;
using PetFamily.Domain.Volunteer;
using PetFamily.Domain.Volunteer.ValueObjects;

namespace PetFamily.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler(IVolunteersRepository volunteersRepository)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository ?? throw new ArgumentNullException(nameof(volunteersRepository));

    public async Task<Result<Guid, Error>> HandleAsync(
        CreateVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        // валидация

        var emailResult = Email.Create(command.Email);

        if (emailResult.IsFailure)
        {
            return emailResult.Error;
        }

        var existedVolunteer = await _volunteersRepository.GetByEmailAsync(emailResult.Value, cancellationToken);

        if (existedVolunteer.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExist();
        }

        var volunteerId = VolunteerId.NewVolunteerId();

        var fullNameResult = FullName.Create(command.FirstName, command.LastName, command.MiddleName);

        if (fullNameResult.IsFailure)
        {
            return fullNameResult.Error;
        }

        var descriptionResult = Description.Create(command.Description);

        if (descriptionResult.IsFailure)
        {
            return descriptionResult.Error;
        }

        var phoneNumberResult = PhoneNumber.Create(command.PhoneNumber);

        if (phoneNumberResult.IsFailure)
        {
            return phoneNumberResult.Error;
        }

        var experienceResult = Experience.Create(0);

        if (experienceResult.IsFailure)
        {
            return experienceResult.Error;
        }

        List<SocialNetwork> socialNetworksList = [];

        foreach (var socialNetwork in command.SocialNetworks)
        {
            var socialNetworkResult = SocialNetwork.Create(socialNetwork.Name, socialNetwork.Url);

            if (socialNetworkResult.IsFailure)
            {
                return socialNetworkResult.Error;
            }

            socialNetworksList.Add(socialNetworkResult.Value);
        }

        List<PaymentDetails> paymentDetailsList = [];

        foreach (var paymentDetail in command.PaymentDetails)
        {
            var paymentDetailResult = PaymentDetails.Create(paymentDetail.Name, paymentDetail.Description);

            if (paymentDetailResult.IsFailure)
            {
                return paymentDetailResult.Error;
            }

            paymentDetailsList.Add(paymentDetailResult.Value);
        }

        var volunteer = new Volunteer(
            volunteerId,
            fullNameResult.Value,
            descriptionResult.Value,
            command.Gender,
            phoneNumberResult.Value,
            emailResult.Value,
            experienceResult.Value,
            socialNetworksList,
            paymentDetailsList
        );

        await _volunteersRepository.AddAsync(volunteer, cancellationToken);

        return (Guid)volunteer.Id;
    }
}
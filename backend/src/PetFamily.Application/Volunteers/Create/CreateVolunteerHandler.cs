using CSharpFunctionalExtensions;
using FluentValidation;
using PetFamily.Application.Extensions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Application.Volunteers.Create;

public class CreateVolunteerHandler(
    IVolunteersRepository volunteersRepository,
    IValidator<CreateVolunteerCommand> validator
)
{
    private readonly IVolunteersRepository _volunteersRepository = volunteersRepository ?? throw new ArgumentNullException(nameof(volunteersRepository));
    private readonly IValidator<CreateVolunteerCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Result<Guid, ErrorList>> HandleAsync(
        CreateVolunteerCommand command, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var email = Email.Create(command.Email).Value;

        var existedVolunteer = await _volunteersRepository.GetByEmailAsync(email, cancellationToken);

        if (existedVolunteer.IsSuccess)
        {
            return Errors.Volunteer.AlreadyExist().ToErrorList();
        }

        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName).Value;
        var description = Description.Create(command.Description).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var gender = Gender.Create(command.Gender).Value;
        var experience = Experience.Create(0).Value;
        var socialNetworks = command.SocialNetworks.Select(x => SocialNetwork.Create(x.Name, x.Url).Value);
        var paymentDetails = command.PaymentDetails.Select(x => PaymentDetails.Create(x.Name, x.Description).Value);

        var volunteerId = VolunteerId.NewVolunteerId();

        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            gender,
            phoneNumber,
            email,
            experience,
            socialNetworks,
            paymentDetails
        );

        await _volunteersRepository.AddAsync(volunteer, cancellationToken);

        return (Guid)volunteer.Id;
    }
}
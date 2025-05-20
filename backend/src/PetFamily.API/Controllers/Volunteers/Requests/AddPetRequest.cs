using PetFamily.Application.DTOs.Shared;
using PetFamily.Domain.Shared.Enums;
using PetFamily.Domain.Volunteer.Pet.Enums;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record AddPetRequest(
    string Name,
    string? Description,
    Genders Gender,
    Guid SpeciesId,
    Guid BreedId,
    string Color,
    float Weight,
    float Height,
    string? HealthInfo,
    HelpStatuses HelpStatus,
    AddressDto Address,
    DateOnly BirthDate,
    bool IsNeutered,
    bool IsVaccinated,
    string VolunteerPhoneNumber);
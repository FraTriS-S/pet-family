using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.API.Processors;
using PetFamily.Application.DTOs.Shared;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.Pets.Add;
using PetFamily.Application.Volunteers.Pets.AddPhotos;
using PetFamily.Application.Volunteers.Pets.MovePosition;
using PetFamily.Application.Volunteers.Pets.RemovePhotos;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdatePaymentDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetworks;

namespace PetFamily.API.Controllers.Volunteers;

public class VolunteersController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult<Guid>> UpdateMainInfo(
        [FromServices] UpdateVolunteerMainInfoHandler handler,
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerMainInfoRequest(id, dto);

        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/social-networks")]
    public async Task<ActionResult<Guid>> UpdateSocialNetworks(
        [FromServices] UpdateVolunteerSocialNetworksHandler handler,
        [FromRoute] Guid id,
        [FromBody] IEnumerable<SocialNetworkDto> socialNetworks,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerSocialNetworksRequest(id, socialNetworks);

        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpPut("{id:guid}/payment-details")]
    public async Task<ActionResult<Guid>> UpdatePaymentDetails(
        [FromServices] UpdateVolunteerPaymentDetailsHandler handler,
        [FromRoute] Guid id,
        [FromBody] IEnumerable<PaymentDetailsDto> paymentDetails,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerPaymentDetailsRequest(id, paymentDetails);

        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteVolunteerHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);

        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{id:guid}/hard")]
    public async Task<ActionResult<Guid>> HardDelete(
        [FromServices] HardDeleteVolunteerHandler handler,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new DeleteVolunteerRequest(id);

        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{id:guid}/pet")]
    public async Task<ActionResult<Guid>> AddPet(
        [FromServices] AddPetHandler handler,
        [FromRoute] Guid id,
        [FromForm] AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddPetCommand(
            id,
            request.Name,
            request.Description,
            request.Gender,
            request.SpeciesId,
            request.BreedId,
            request.Color,
            request.Weight,
            request.Height,
            request.HealthInfo,
            request.HelpStatus,
            request.Address,
            request.BirthDate,
            request.IsNeutered,
            request.IsVaccinated,
            request.VolunteerPhoneNumber);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photo")]
    public async Task<IActionResult> UploadFiles(
        [FromServices] UploadPetPhotosHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IFormFileCollection photos,
        CancellationToken cancellationToken)
    {
        await using var fileProcessor = new FormFileProcessor();

        var photoDtos = fileProcessor.Process(photos);

        var command = new UploadPetPhotosCommand(volunteerId, petId, photoDtos);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpDelete("{volunteerId:guid}/pet/{petId:guid}/photo")]
    public async Task<IActionResult> RemoveFiles(
        [FromServices] RemovePetPhotosHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        IEnumerable<string> photosNames,
        CancellationToken cancellationToken)
    {
        var command = new RemovePetPhotosCommand(volunteerId, petId, photosNames);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/position")]
    public async Task<IActionResult> MovePetPosition(
        [FromServices] MovePetPositionHandler handler,
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromBody] int position,
        CancellationToken cancellationToken)
    {
        var command = new MovePetPositionCommand(volunteerId, petId, position);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToResponse();
    }
}
using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Extensions;
using PetFamily.API.Requests.Volunteers;
using PetFamily.Application.Volunteers.CreateVolunteer;

namespace PetFamily.API.Controllers;

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
}
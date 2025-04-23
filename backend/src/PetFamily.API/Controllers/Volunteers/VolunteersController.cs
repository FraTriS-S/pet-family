using Microsoft.AspNetCore.Mvc;
using PetFamily.API.Controllers.Volunteers.Requests;
using PetFamily.API.Extensions;
using PetFamily.Application.Volunteers.Create;

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
}
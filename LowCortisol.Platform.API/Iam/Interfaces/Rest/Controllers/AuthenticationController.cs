using LowCortisol.Platform.API.Iam.Application.CommandServices;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Resources;
using LowCortisol.Platform.API.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LowCortisol.Platform.API.Iam.Interfaces.Rest.Controllers;

[ApiController]
[Route("api/v1/authentication")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationCommandService _authenticationCommandService;

    public AuthenticationController(IAuthenticationCommandService authenticationCommandService)
    {
        _authenticationCommandService = authenticationCommandService;
    }

    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUp(SignUpResource resource, CancellationToken cancellationToken)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _authenticationCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResult(
            this,
            result,
            AuthenticatedUserResourceFromResultAssembler.ToResourceFromResult,
            authenticated => Created($"/api/v1/users/{authenticated.User.Id}", authenticated));
    }

    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn(SignInResource resource, CancellationToken cancellationToken)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _authenticationCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResult(
            this,
            result,
            AuthenticatedUserResourceFromResultAssembler.ToResourceFromResult);
    }
}

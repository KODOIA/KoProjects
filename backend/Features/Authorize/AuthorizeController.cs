using Microsoft.AspNetCore.Mvc;
using Features.Authorize.Add;
using Swashbuckle.AspNetCore.Annotations;

namespace Features.Authorize;

[ApiController]
[Route("api/authorize")]
[SwaggerTag("Handles authorization operations. Supports the PKCE flow by exchanging an authorization code and code verifier for access and refresh tokens.")]
public class AuthorizeController : ControllerBase
{
    private readonly AddAuthorizeHandler _addAuthorizeHandler;

    public AuthorizeController(AddAuthorizeHandler addAuthorizeHandler)
    {
        _addAuthorizeHandler = addAuthorizeHandler;
    }

    [SwaggerOperation(
        Summary = "Exchanges an authorization code for tokens.",
        Description = "Accepts an authorization code and code verifier (PKCE) and returns access token, refresh token, expiry and token type.")]
    [HttpPost]
    [ProducesResponseType(typeof(AddAuthorizeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddAuthorize(
        [FromBody] AddAuthorizeRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _addAuthorizeHandler.Handle(request, cancellationToken);
        return Ok(response);
    }
}

using Microsoft.AspNetCore.Mvc;
using Features.Token.Post;
using Swashbuckle.AspNetCore.Annotations;

namespace Features.Token;

[ApiController]
[Route("api/token")]
[SwaggerTag("Handles token operations. Supports authorization_code (PKCE) and refresh_token grant types.")]
public class TokenController : ControllerBase
{
    private readonly TokenHandler _tokenHandler;

    public TokenController(TokenHandler tokenHandler)
    {
        _tokenHandler = tokenHandler;
    }

    [SwaggerOperation(
        Summary = "Token",
        Description = "Exchanges an authorization code (PKCE) or refresh token for access and refresh tokens. Pass grant_type as 'authorization_code' or 'refresh_token'.")]
    [HttpPost]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Token(
        [FromBody] TokenRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _tokenHandler.Handle(request, cancellationToken);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}

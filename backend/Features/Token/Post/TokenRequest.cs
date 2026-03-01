namespace Features.Token.Post;

public sealed class TokenRequest
{
    public required string GrantType { get; set; }

    // authorization_code
    public string? Code { get; set; }
    public string? CodeVerifier { get; set; }

    // refresh_token
    public string? RefreshToken { get; set; }
}

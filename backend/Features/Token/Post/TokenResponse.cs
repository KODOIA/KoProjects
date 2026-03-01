namespace Features.Token.Post;

public sealed class TokenResponse
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public required int ExpiresIn { get; set; }
    public required string TokenType { get; set; }
}

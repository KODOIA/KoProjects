namespace Features.Authorize.Add;

public sealed class AddAuthorizeRequest
{
    public required string Code { get; set; }
    public required string CodeVerifier { get; set; }
}
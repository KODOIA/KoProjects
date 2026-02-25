namespace Features.Authorize.Add;

public sealed class AddAuthorizeHandler
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AddAuthorizeHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AddAuthorizeResponse> Handle(AddAuthorizeRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.Code);
        Console.WriteLine(request.CodeVerifier);

        return new AddAuthorizeResponse
        {
            AccessToken = "test_access_token",
            RefreshToken = "test_refresh_token",
            ExpiresIn = 3600,
            TokenType = "Bearer"
        };
    }
}
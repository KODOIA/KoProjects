using System.Text;
using System.Text.Json;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Features.Token.Post;

public sealed class TokenHandler
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _dbContext;

    public TokenHandler(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        AppDbContext dbContext)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public Task<TokenResponse> Handle(TokenRequest request, CancellationToken cancellationToken) =>
        request.GrantType switch
        {
            "authorization_code" => HandleAuthorizationCode(request, cancellationToken),
            "refresh_token" => HandleRefreshToken(request, cancellationToken),
            _ => throw new InvalidOperationException($"Unsupported grant_type: {request.GrantType}")
        };

    private async Task<TokenResponse> HandleAuthorizationCode(TokenRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.CodeVerifier))
            throw new InvalidOperationException("code and code_verifier are required for authorization_code grant.");

        var tokenEndpoint = _configuration["Authorization:TokenEndpoint"]!;
        var clientId = _configuration["Authorization:ClientId"]!;
        var clientSecret = _configuration["Authorization:ClientSecret"]!;
        var redirectUri = _configuration["Authorization:RedirectUri"]!;

        var tokenData = await ExchangeToken(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = request.Code,
            ["code_verifier"] = request.CodeVerifier,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri
        }, tokenEndpoint, cancellationToken);

        var userClaims = DecodeJwtPayload(tokenData.AccessToken);
        await UpsertUser(userClaims, cancellationToken);

        return MapToResponse(tokenData);
    }

    private async Task<TokenResponse> HandleRefreshToken(TokenRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.RefreshToken))
            throw new InvalidOperationException("refresh_token is required for refresh_token grant.");

        var tokenEndpoint = _configuration["Authorization:TokenEndpoint"]!;
        var clientId = _configuration["Authorization:ClientId"]!;
        var clientSecret = _configuration["Authorization:ClientSecret"]!;

        var tokenData = await ExchangeToken(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = request.RefreshToken,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        }, tokenEndpoint, cancellationToken);

        return MapToResponse(tokenData);
    }

    private async Task<AuthorizationServerResponse> ExchangeToken(
        Dictionary<string, string> formFields,
        string tokenEndpoint,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();
        var formContent = new FormUrlEncodedContent(formFields);
        var httpResponse = await client.PostAsync(tokenEndpoint, formContent, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode)
        {
            var error = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Token request failed: {error}");
        }

        var json = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<AuthorizationServerResponse>(json)
            ?? throw new InvalidOperationException("Failed to deserialize token response.");
    }

    private static TokenResponse MapToResponse(AuthorizationServerResponse tokenData) =>
        new()
        {
            AccessToken = tokenData.AccessToken,
            RefreshToken = tokenData.RefreshToken,
            ExpiresIn = tokenData.ExpiresIn,
            TokenType = tokenData.TokenType
        };

    private static Dictionary<string, JsonElement> DecodeJwtPayload(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length < 2)
            throw new InvalidOperationException("Invalid JWT format.");

        var payload = parts[1];
        var remainder = payload.Length % 4;
        if (remainder > 0)
            payload += new string('=', 4 - remainder);

        var bytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
        return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Encoding.UTF8.GetString(bytes))
            ?? [];
    }

    private async Task UpsertUser(Dictionary<string, JsonElement> claims, CancellationToken cancellationToken)
    {
        if (!claims.TryGetValue("sub", out var subElement))
            return;

        if (!Guid.TryParse(subElement.GetString(), out var userId))
            return;

        var existing = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        var givenName = claims.TryGetValue("given_name", out var gn) ? gn.GetString() : null;
        var familyName = claims.TryGetValue("family_name", out var fn) ? fn.GetString() : null;
        var email = claims.TryGetValue("email", out var em) ? em.GetString() : null;
        var preferredUsername = claims.TryGetValue("preferred_username", out var pu) ? pu.GetString() : null;

        if (existing is null)
        {
            _dbContext.Users.Add(new User
            {
                Id = userId,
                GivenName = givenName,
                FamilyName = familyName,
                Email = email,
                PreferredUsername = preferredUsername,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });
        }
        else
        {
            existing.GivenName = givenName;
            existing.FamilyName = familyName;
            existing.Email = email;
            existing.PreferredUsername = preferredUsername;
            existing.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

using System.Text.Json;
using dotnet.Common;
using dotnet.Http.Responses;
using dotnet.Http.Responses.Auth;

namespace dotnet.Services;

public class GoogleService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IServiceRegistration
{
    private const string TokenEndpoint = "https://oauth2.googleapis.com/token";
    private const string UserInfoEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";

    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IConfiguration _configuration = configuration;

    public async Task<GoogleTokenResponse> ExchangeCodeForTokenAsync(string code, string redirectUri)
    {
        var client = _httpClientFactory.CreateClient();

        var parameters = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _configuration["Authentication:Google:ClientId"]! },
            { "client_secret", _configuration["Authentication:Google:ClientSecret"]! },
            { "redirect_uri", redirectUri },
            { "grant_type", "authorization_code" }
        };

        var response = await client.PostAsync(
            TokenEndpoint,
            new FormUrlEncodedContent(parameters)
        );

        if (!response.IsSuccessStatusCode)
            ApiResponse.Fail("Failed to exchange authorization code");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleTokenResponse>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }

    public async Task<GoogleUserInfo> GetUserInfoAsync(string accessToken)
    {
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

        var response = await client.GetAsync(UserInfoEndpoint);

        if (!response.IsSuccessStatusCode)
            ApiResponse.Fail("Failed to get user info");

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GoogleUserInfo>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
    }
}


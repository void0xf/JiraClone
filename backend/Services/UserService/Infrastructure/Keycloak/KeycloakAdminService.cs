using System.Net;
using System.Text;
using System.Text.Json;
using SharedKernel;

namespace UserService.Infrastructure.Keycloak;

public interface IKeycloakAdminService
{
    Task<Result<string>> CreateUserAsync(string email);
    Task<Result<bool>> IsEmailExistsAndUnverified(string email);
    Task<Result<bool>> IsEmailVerified(string email);
    Task<Result<bool>> ResendVerificationEmailAsync(string email);
}

public class KeycloakAdminService : IKeycloakAdminService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _baseUrl;
    private readonly string _realm;
    private readonly string _clientId;
    private readonly string _clientSecret;

    public KeycloakAdminService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _baseUrl = configuration["Keycloak:BaseUrl"]!;
        _realm = configuration["Keycloak:RealmName"]!;
        _clientId = configuration["Keycloak:AdminClientId"]!;
        _clientSecret = configuration["Keycloak:AdminClientSecret"]!;
    }

    private async Task<Result<string>> GetAdminTokenAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var tokenUrl = $"{_baseUrl}/realms/{_realm}/protocol/openid-connect/token";

            var formData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _clientId },
                { "client_secret", _clientSecret }
            };

            var content = new FormUrlEncodedContent(formData);
            var response = await client.PostAsync(tokenUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result<string>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakTokenFailed,
                        $"Failed to get Keycloak admin token. Status: {response.StatusCode}",
                        "Failed to authenticate with identity provider"));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(jsonResponse);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.access_token))
            {
                return Result<string>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakTokenFailed,
                        "Invalid token response from Keycloak",
                        "Failed to authenticate with identity provider"));
            }

            return Result<string>.Success(tokenResponse.access_token);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                Error.Unexpected(
                    ErrorCode.KeycloakTokenFailed,
                    "Exception while getting admin token",
                    "Failed to authenticate with identity provider",
                    ex));
        }
    }

    public async Task<Result<string>> CreateUserAsync(string email)
    {
        try
        {
            var tokenResult = await GetAdminTokenAsync();
            if (tokenResult.IsFailure)
            {
                return Result<string>.Failure(tokenResult.Error!);
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

            var user = new
            {
                email,
                username= email,
                emailVerified = false,
                enabled = true,
                requiredActions = new[] { "VERIFY_EMAIL", "UPDATE_PASSWORD" }
            };

            var userJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}/admin/realms/{_realm}/users", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return Result<string>.Failure(
                        Error.Conflict(
                            ErrorCode.UserAlreadyExists,
                            $"Keycloak user already exists for {email}",
                            "User already exists"));
                }

                return Result<string>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCreationFailed,
                        $"Failed to create user in Keycloak. Status: {response.StatusCode}, Error: {errorContent}",
                        "Failed to create user account"));
            }

            var location = response.Headers.Location?.ToString();
            if (string.IsNullOrEmpty(location))
            {
                return Result<string>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCreationFailed,
                        "Keycloak did not return user location",
                        "Failed to create user account"));
            }

            var userId = location.Split('/').Last();

            var executeActionsUrl = $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/execute-actions-email";
            var actions = new[] { "VERIFY_EMAIL", "UPDATE_PASSWORD" };
            var actionsJson = JsonSerializer.Serialize(actions);
            var emailContent = new StringContent(actionsJson, Encoding.UTF8, "application/json");

            var executeResponse = await client.PutAsync(executeActionsUrl, emailContent);

            if (!executeResponse.IsSuccessStatusCode)
            {
                var errorContent = await executeResponse.Content.ReadAsStringAsync();
                // Log the error but don't fail the user creation
                Console.WriteLine($"Warning: Failed to send verification email. Status: {executeResponse.StatusCode}, Error: {errorContent}");
            }

            return Result<string>.Success(userId);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(
                Error.Unexpected(
                    ErrorCode.KeycloakUserCreationFailed,
                    "Exception while creating user",
                    "Failed to create user account",
                    ex));
        }
    }

    public async Task<Result<bool>> IsEmailExistsAndUnverified(string email)
    {
        try
        {
            var tokenResult = await GetAdminTokenAsync();
            if (tokenResult.IsFailure)
            {
                return Result<bool>.Failure(tokenResult.Error!);
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

            var url = $"{_baseUrl}/admin/realms/{_realm}/users?email={email}&exact=true";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result<bool>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCheckFailed,
                        $"Failed to check if email exists. Status: {response.StatusCode}, Error: {errorContent}",
                        "Failed to verify email availability"));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<KeycloakUser>>(jsonResponse);

            if (users != null && users.Any())
            {
                var user = users.First();
                return Result<bool>.Success(!user.emailVerified);
            }

            return Result<bool>.Success(false);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(
                Error.Unexpected(
                    ErrorCode.KeycloakUserCheckFailed,
                    "Exception while checking email",
                    "Failed to verify email availability",
                    ex));
        }
    }

    public async Task<Result<bool>> IsEmailVerified(string email)
    {
        try
        {
            var tokenResult = await GetAdminTokenAsync();
            if (tokenResult.IsFailure)
            {
                return Result<bool>.Failure(tokenResult.Error!);
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

            var url = $"{_baseUrl}/admin/realms/{_realm}/users?email={email}&exact=true";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result<bool>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCheckFailed,
                        $"Failed to check email verification status. Status: {response.StatusCode}, Error: {errorContent}",
                        "Failed to verify email status"));
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<KeycloakUser>>(jsonResponse);

            if (users == null || !users.Any())
            {
                return Result<bool>.Failure(
                    Error.NotFound(
                        ErrorCode.UserNotFound,
                        $"User with email {email} not found in Keycloak",
                        "User not found"));
            }

            var user = users.First();
            return Result<bool>.Success(user.emailVerified);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(
                Error.Unexpected(
                    ErrorCode.KeycloakUserCheckFailed,
                    "Exception while checking email verification",
                    "Failed to verify email status",
                    ex));
        }
    }

    public async Task<Result<bool>> ResendVerificationEmailAsync(string email)
    {
        try
        {
            var tokenResult = await GetAdminTokenAsync();
            if (tokenResult.IsFailure)
            {
                return Result<bool>.Failure(tokenResult.Error!);
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResult.Value);

            var userSearchUrl = $"{_baseUrl}/admin/realms/{_realm}/users?email={email}&exact=true";
            var searchResponse = await client.GetAsync(userSearchUrl);

            if (!searchResponse.IsSuccessStatusCode)
            {
                var errorContent = await searchResponse.Content.ReadAsStringAsync();
                return Result<bool>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCheckFailed,
                        $"Failed to find user. Status: {searchResponse.StatusCode}, Error: {errorContent}",
                        "Failed to find user"));
            }

            var jsonResponse = await searchResponse.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<KeycloakUser>>(jsonResponse);

            if (users == null || !users.Any())
            {
                return Result<bool>.Failure(
                    Error.NotFound(
                        ErrorCode.UserNotFound,
                        $"User with email {email} not found in Keycloak",
                        "User not found"));
            }

            var userId = users.First().id;

            var executeActionsUrl = $"{_baseUrl}/admin/realms/{_realm}/users/{userId}/execute-actions-email";
            var actions = new[] { "VERIFY_EMAIL", "UPDATE_PASSWORD" };
            var actionsJson = JsonSerializer.Serialize(actions);
            var content = new StringContent(actionsJson, Encoding.UTF8, "application/json");

            var executeResponse = await client.PutAsync(executeActionsUrl, content);

            if (!executeResponse.IsSuccessStatusCode)
            {
                var errorContent = await executeResponse.Content.ReadAsStringAsync();
                return Result<bool>.Failure(
                    Error.Unexpected(
                        ErrorCode.KeycloakUserCreationFailed,
                        $"Failed to send verification email. Status: {executeResponse.StatusCode}, Error: {errorContent}",
                        "Failed to send verification email"));
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(
                Error.Unexpected(
                    ErrorCode.KeycloakUserCreationFailed,
                    "Exception while sending verification email",
                    "Failed to send verification email",
                    ex));
        }
    }

    private class TokenResponse
    {
    public string access_token { get; set; } = null!;
    public int expires_in { get; set; }
    }

    private class KeycloakUser
    {
        public string id { get; set; } = null!;
        public string? email { get; set; }
        public bool emailVerified { get; set; }
    }
}
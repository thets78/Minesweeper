using MinesWeeper.Models;
namespace MinesWeeper.Services;

public class UserServiceApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public UserServiceApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ApiUser?> LoginUserAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/Users/login", new { Username = username, Password = password });
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ApiUser>();
        }
        return null;
    }

    public async Task<ApiUser?> LoginUserAsync()
    {
        Console.WriteLine("--- UserServiceApiClient - LoginUserAsync -----------------------------------------------");
        if (_configuration["MINES_WEEPER_API_USER"] is string username && _configuration["MINES_WEEPER_API_PASSWORD"] is string password)
        {
            Console.WriteLine($"user: \"{username}\" - password: \"{password.Substring(0, Math.Min(5, password.Length))}******\"");
            var userResult = await LoginUserAsync(username, password);

            if (userResult is not null)
            {
                Console.WriteLine($"return user: \"{userResult.Username}\" - token: \"{userResult.Token?.Substring(0, Math.Min(5, userResult.Token.Length))}******************\"");
            }
            else
            {
                Console.WriteLine("No user found in configuration");
            }

            return userResult;
        }

        Console.WriteLine("No user and password found in configuration");
        return null;
    }

    public async Task<bool> LogoutUserAsync(string username)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/Users/logout", new { Username = username });
        return response.IsSuccessStatusCode;
    }
}
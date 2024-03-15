using System.Net;
using System.Net.Http.Headers;
using MinesWeeper.Api.Models;

namespace MinesWeeper.Services;

public class MinesWeeperHighscoreApiClient
{
    private readonly HttpClient _httpClient;
    public MinesWeeperHighscoreApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<HighScore>?> GetHighscoresByFieldTypeAsync(FieldType fieldType, string token)
    {
        Console.WriteLine("--- MinesWeeperHighscoreApiClient - GetHighscoresByFieldTypeAsync -----------------------");
        Console.WriteLine($"token: {token.Substring(0, Math.Min(5, token.Length))}******************");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response =  await _httpClient.GetAsync($"api/v1/MinesWeeper/highscores/fieldtype?FieldWidth={fieldType.FieldWidth}&FieldHeight={fieldType.FieldHeight}&MineCount={fieldType.MineCount}");
    
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<IEnumerable<HighScore>>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        return null;
    }

    public async Task<HighScore?> AddHighscoreAsync(HighScore highScore, string token)
    {
        Console.WriteLine("--- MinesWeeperHighscoreApiClient - AddHighscoreAsync -----------------------------------");
        Console.WriteLine($"token: {token.Substring(0, Math.Min(5, token.Length))}******************");
        Console.WriteLine($"highScore: {highScore.Time} - {highScore.Username} - {highScore.FieldWidth}x{highScore.FieldHeight} - Mines: {highScore.MineCount}");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("api/v1/MinesWeeper/highscore", highScore);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<HighScore>();
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }

        return null;
    }
}
using System.Text.Json;

namespace Minesweeper;

public enum GameGridSquareType
{
    Empty,
    Mine,
    Number
}

public enum GameType
{
    Easy,
    Medium,
    Hard
}

public enum GameState 
{
    NotStarted,
    InProgress,
    Won,
    Lost
}

public static class GameTypeExtensions
{
    public static string ToFriendlyString(this GameType gameType)
    {
        return gameType switch
        {
            GameType.Easy => "Easy",
            GameType.Medium => "Medium",
            GameType.Hard => "Hard",
            _ => "Unknown"
        };
    }
}
public class Game
{
    public int GridWidth { get; set; } = 10;
    public int GridHeight { get; set; } = 10;
    public int MineCount { get; set; } = 10;
    public GameType GameType { get; set; } = GameType.Easy;
    public TimeSpan Time { get; set; } = TimeSpan.Zero;
    public GameState GameState { get; set; } = GameState.NotStarted;

    public override string ToString()
    {
        return $"{GameType.ToFriendlyString()} - {Time} - {GameState} - {GridWidth}x{GridHeight} - {MineCount} mines";
    }

    public static Game SetupGame(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.Easy:
                return new Game { GridWidth = 10, GridHeight = 10, MineCount = 10, GameType = GameType.Easy, GameState = GameState.NotStarted};
            case GameType.Medium:
                return new Game { GridWidth = 15, GridHeight = 12, MineCount = 30, GameType = GameType.Medium, GameState = GameState.NotStarted };
            case GameType.Hard:
                return new Game { GridWidth = 20, GridHeight = 14, MineCount = 60, GameType = GameType.Hard, GameState = GameState.NotStarted };
            default:
                return new Game { GridWidth = 10, GridHeight = 10, MineCount = 10, GameType = GameType.Easy, GameState = GameState.NotStarted };
        }
    }
}

public class GameGridSquare
{
    public int Id { get; set; } = 0;
    public GameGridSquareType Type { get; set; } = GameGridSquareType.Empty;
    public bool IsRevealed { get; set; } = false;
    public bool IsFlagged { get; set; } = false;
    public bool IsChecked { get; set; } = false;
    public int AdjacentMineCount { get; set; } = 0;

    public string Class
    {
        get
        {
            if (IsRevealed)
            {
                if (Type == GameGridSquareType.Mine)
                {
                    return "mine";
                }
                else if (AdjacentMineCount > 0)
                {
                    if (AdjacentMineCount == 1)
                        return $"number green";
                    else if (AdjacentMineCount == 2)
                        return $"number blue";
                    else
                        return $"number red";
                }
                else
                {
                    return $"valid";
                }
            }
            else
            {
                if (IsFlagged)
                {
                    return "flag";
                }
                else if (IsChecked)
                {
                    return "checked";
                }
                else
                {
                    return "hidden";
                }
            }
        }
    }
}

public class HighScore
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public TimeSpan Time { get; set; }
    public GameType GameType { get; set; }
    public static async Task<List<HighScore>> GetHighScoresAsync(GameType gameType)
    {
        var fileName = $"highscores_{gameType}.json";
        try {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
            var json = await File.ReadAllTextAsync(fileName);
            return JsonSerializer.Deserialize<List<HighScore>>(json) ?? new List<HighScore>();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<HighScore>();
        }
    }
    public static async Task SaveHighScoreAsync(HighScore highScore)
    {
        var fileName = $"highscores_{highScore.GameType}.json";
        try {
            List<HighScore> highScores = await GetHighScoresAsync(highScore.GameType);
            highScores.Add(highScore);
            highScores = highScores.OrderBy(x => x.Time).Take(10).ToList();
            highScores = highScores.Where(x => x.Time != TimeSpan.Zero).ToList();
            var json = JsonSerializer.Serialize(highScores);
            File.WriteAllText(fileName, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}


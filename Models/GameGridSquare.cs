using System.Text.Json;

namespace Minesweeper;

public enum GameGridSquareType
{
    Empty,
    Mine,
    Number
}

// Minesweeper on Linux
// Easy   =>  8x8  with 10 mines ==> 10 /  8*8  = 15.625% chance of mine
// Medium => 16x16 with 40 mines ==> 40 / 16*16 = 15.625% chance of mine
// Hard   => 16x30 with 99 mines ==> 99 / 16*30 = 20.625% chance of mine

// My implementation
// Easy   => 10x10 with 10 mines ==> 10 / 10*10 = 10.00% chance of mine
// Medium => 15x12 with 30 mines ==> 30 / 15*12 = 16.67% chance of mine
// Hard   => 20x14 with 60 mines ==> 60 / 20*14 = 21.43% chance of mine

public enum GameType
{
    Easy,
    Medium,
    Hard
}

public enum GameState 
{
    NotStarted,
    RequestStart,
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
    public Guid Id { get; set; }

    public override string ToString()
    {
        return $"{GameType.ToFriendlyString()} - {Time} - {GameState} - {GridWidth}x{GridHeight} - {MineCount} mines";
    }

    public static Game SetupGame(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.Easy:
                return new Game { GridWidth = 10, GridHeight = 10, MineCount = 10, GameType = GameType.Easy, GameState = GameState.NotStarted, Id = Guid.NewGuid()};
            case GameType.Medium:
                return new Game { GridWidth = 15, GridHeight = 12, MineCount = 30, GameType = GameType.Medium, GameState = GameState.NotStarted, Id = Guid.NewGuid() };
            case GameType.Hard:
                return new Game { GridWidth = 20, GridHeight = 14, MineCount = 60, GameType = GameType.Hard, GameState = GameState.NotStarted, Id = Guid.NewGuid() };
            default:
                return new Game { GridWidth = 10, GridHeight = 10, MineCount = 10, GameType = GameType.Easy, GameState = GameState.NotStarted, Id = Guid.NewGuid() };
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
    public DateTime? Date { get; set; }
    
    public static async Task<List<HighScore>> GetHighScoresAsync(GameType gameType)
    {
        var fileName = $"/data/highscores_{gameType}.json";
        var fileNameLocal = $"highscores_{gameType}.json";

        try {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            fileName = fileNameLocal;
        }

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
        var fileName = $"/data/highscores_{highScore.GameType}.json";
        var fileNameLocal = $"highscores_{highScore.GameType}.json";

        try {
            if (!File.Exists(fileName))
                File.Create(fileName).Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            fileName = fileNameLocal;
        }


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


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
public class GameGrid
{
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public int MineCount { get; set; }
    public GameType GameType { get; set; }

    public static GameGrid SetupGame(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.Easy:
                return new GameGrid{ GridWidth = 10, GridHeight=10, MineCount = 10, GameType = GameType.Easy};
            case GameType.Medium:
                return new GameGrid{ GridWidth = 15, GridHeight=10, MineCount = 30, GameType = GameType.Medium};
            case GameType.Hard:
                return new GameGrid{ GridWidth = 20, GridHeight=10, MineCount = 60, GameType = GameType.Hard};
            default:
                return new GameGrid{ GridWidth = 10, GridHeight=10, MineCount = 10, GameType = GameType.Easy};
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
                    if (AdjacentMineCount ==  1)
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

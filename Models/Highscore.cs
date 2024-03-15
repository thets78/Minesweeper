using System.ComponentModel.DataAnnotations;

namespace MinesWeeper.Api.Models;

public class HighScore
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Username { get; set; }
    [Required]
    public TimeSpan Time { get; set; }
    [Required]
    public int FieldWidth { get; set; }
    [Required]
    public int FieldHeight { get; set; }
    [Required]
    public int MineCount { get; set; }
    [Required]
    public DateTime Date { get; set; }

}
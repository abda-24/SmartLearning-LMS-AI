using System.ComponentModel.DataAnnotations;

public class UpdateCourseDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? ThumbnailUrl { get; set; } 
    [Range(0, 100000)]
    public decimal Price { get; set; }
}
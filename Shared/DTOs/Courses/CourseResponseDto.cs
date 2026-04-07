public class CourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } 
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string? ThumbnailUrl { get; set; } 
    public decimal Rating { get; set; } 
    public string InstructorName { get; set; } = string.Empty;
    public int ModulesCount { get; set; }
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
}
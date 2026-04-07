namespace Domain.Entities
{
    public class Course : BaseEntity<int>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ThumbnailUrl { get; set; }
        public decimal Rating { get; set; } = 0;

        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Modules> Modules { get; set; } = new List<Modules>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
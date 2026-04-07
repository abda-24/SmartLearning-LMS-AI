using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public ICollection<Course> AuthoredCourses { get; set; } = new List<Course>();

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public Instructor? InstructorProfile { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = new();
    }
}
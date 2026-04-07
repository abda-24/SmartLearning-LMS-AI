using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Instructor:BaseEntity<int>
    {
        public string Bio { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public decimal Rating { get; set; } = 0;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}

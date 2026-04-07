using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Wishlist:BaseEntity<int>
    {
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}

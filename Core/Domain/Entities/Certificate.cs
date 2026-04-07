using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Certificate
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = string.Empty; 
        public int CourseId { get; set; }
        public string CertificateUrl { get; set; } = string.Empty; 
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

                public Course Course { get; set; } = null!;
    }
}

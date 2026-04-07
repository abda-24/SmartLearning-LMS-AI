using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Courses
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Course title is required.")]
        [MaxLength(200, ErrorMessage = "Course title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course description is required.")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000.")]
        public decimal Price { get; set; }

                [MaxLength(500, ErrorMessage = "Thumbnail URL cannot exceed 500 characters.")]
        public string? ThumbnailUrl { get; set; }

        [Required(ErrorMessage = "Instructor ID is required.")]
        public int InstructorId { get; set; } 
        public int CategoryId { get; set; }
    }
}
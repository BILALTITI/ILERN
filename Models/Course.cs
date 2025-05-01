 using Microsoft.AspNetCore.Cors.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Assigment2.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, ErrorMessage = "Course Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Degree is required.")]
        [Range(20, 100, ErrorMessage = "Degree must be between 0 and 100.")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "Minimum Degree is required.")]
        [Range(50, 100, ErrorMessage = "Minimum Degree must be between 0 and 100.")]
        public string MinDegree { get; set; }

        [Required(ErrorMessage = "Department selection is required.")]
        public int DepartmentId { get; set; }
        public virtual Department ?Department { get; set; }

        
    }
}

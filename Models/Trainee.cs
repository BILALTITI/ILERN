using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assigment2.Models
{
    public class Trainee
    {
        [Key]
        public int TraineeId { get; set; }

        //        [Required(ErrorMessage = "Name is required.")]
        //      [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        [Required(ErrorMessage = "Tainee Name  is required  .")]

        public string Name { get; set; }

        public string? Image { get; set; }

        [NotMapped]
        public IFormFile?  ImageFile { get; set; }  // For file upload, not stored in DB
        [Required(ErrorMessage = "Trainee Address  is required  .")]

        public string Address { get; set; }

        [Required(ErrorMessage = "Trainee Degree  is required  .")]

        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
        public double Grade { get; set; }

        // Relationship with Department
        [Required(ErrorMessage = "Department Name  is required  .")]
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        // Relationship with Course
        [Required(ErrorMessage = "Course  Name  is required  .")]
        public int? courseId { get; set; }
        public virtual Course? Course { get; set; }
    }
}

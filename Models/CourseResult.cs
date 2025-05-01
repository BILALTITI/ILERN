using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assigment2.Models
{
    public class CourseResult
    {
        [Key]
        public int CourseResultId { get; set; }  // Renamed for consistency

        [Required(ErrorMessage = "Degree is required.")]
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100.")]
        public double Degree { get; set; }

        // Foreign Key for Course
        [Required(ErrorMessage = "Course Name  is required  .")]
        public int CourseId { get; set; }
        public virtual Course ? Course { get; set; }

        // Foreign Key for Trainee
      
        [Required(ErrorMessage = "Trainee  Name  is required  .")]
        public int TraineeId { get; set; }
        public virtual Trainee ? Trainee { get; set; }
    }
}

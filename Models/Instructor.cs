using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assigment2.Models
{
    public class Instructor
    {
       
        public int InstructorId { get; set; }


        //[MaxLength(10)]
        //[Required]
        //[MinLength(4)]
        [Required(ErrorMessage = "Instructor Name is required  .")]
 
        public string Name { get; set; }

        //   [RegularExpression(@"[A-Z]+\.(jpg|png)")]
         
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }  // For file upload, not stored in DB


        //        [Column(TypeName = "decimal(18,2)")]


        //[RegularExpression(@"0-9 {4}")]// from 0 to 9 number and only 4 digts
        //[Required]
        //[Range(0, 10000)]


        // this means we have function called CheckSalary in InstructorController
        //[Remote("CheckSalary","Instructor",ErrorMessage ="Salary Not Valid "/*,AdditionalFields ="Dept_Id"*/)]
        //[Range(300, 3000)]

        [Required(ErrorMessage = "Instructor Salary  is required  .")]
        [Range(300, 10000, ErrorMessage = "Salary must be between 300$  and 10000$.")]

        public decimal Salary { get; set; }

        //[RegularExpression("Irbid |Amman |Zarqa")]
        [Required(ErrorMessage = "Address    is required  .")]
        public string Address { get; set; }

        // Foreign Key لعلاقته بـ Department
        [Required(ErrorMessage = "Department Name  is required  .")]

        public int? DepartmentId { get; set; }
       
        public virtual Department? Department { get; set; }

        // Foreign Key لعلاقته بـ Course
        [Required(ErrorMessage = "Course Name  is required  .")]

        public int? CourseId { get; set; }
        public virtual Course? Course { get; set; }
     }

    }

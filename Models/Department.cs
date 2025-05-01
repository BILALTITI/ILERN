using Assigment2.Models.CustomAttibute;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Assigment2.Models
{

    // Paernt
    public class Department
    {
    //[DataType (DataType.Password)] we can specify the type when use @Html.Editor WILL Apply
        public int DepartmentId { get; set; }
        //    [Display(Name = "Department Name")]to change the name of the label WHEN USE @Html.LabelFor

        //[RegularExpression("Irbid |Amman |Zarqa",ErrorMessage ="Have to input Irbid or amman or zarqa")]
        // [UniqeName(MSG="Own Message ")]//Custom Attribute
        
        [Required(ErrorMessage = "Department Name is required  .")]
         public string Name { get; set; }
        [Required(ErrorMessage = "Manger  Name is required  .")]


        public string Manager { get; set; }


        //public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        //public virtual ICollection<Trainee> Trainees { get; set; } = new List<Trainee>();


    }
}

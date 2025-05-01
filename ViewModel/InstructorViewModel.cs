using Assigment2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment2.ViewModel
{
    public class InstructorViewModel
    {
      public int InstructorID { get; set; }

        public string Name { get; set; }
        public string Image { get; set; }


       

         
        public decimal Salary { get; set; }

      
        public string Address { get; set; }
        public string DepartmentName { get; set; }

        public string CourseName { get; set; }
    }

}

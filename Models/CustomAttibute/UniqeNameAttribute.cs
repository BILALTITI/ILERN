using System.ComponentModel.DataAnnotations;

namespace Assigment2.Models.CustomAttibute
{
    public class UniqeNameAttribute:ValidationAttribute

    {
        // public string MSG { get; set; }//in case we want to pass the message from the user
        //Work Server Side Only 
        protected override ValidationResult? IsValid(object? value ,ValidationContext validationContext)
        {
            using (var Context  = new AppDbContext())
            {
                //if we want get full Department Object 
               //Department department1 =(Department)validationContext.ObjectInstance;
                string Name = value.ToString();
                Department department=Context.Departments.FirstOrDefault(x=>x.Name==Name);

                if (department == null)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Name Already Exsist");
            }

        }
    }
}

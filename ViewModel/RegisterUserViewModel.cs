using Assigment2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Assigment2.Controllers.AccountController;
using Microsoft.AspNetCore.Http;

namespace Assigment2.ViewModel
{
    public class RegisterUserViewModel
    {
        
         
        
       
            public string FullName { get; set; }

           
            public string UserName { get; set; }
         
            public string Email { get; set; }
         
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }

           
            public DateTime DateOfBirth { get; set; }

            [Required(ErrorMessage = "Gender is required")]
            public string Gender { get; set; }

            [Required(ErrorMessage = "Phone Number is required")]
            [Phone(ErrorMessage = "Invalid phone number")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Address is required")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Account type is required")]
            public string Role { get; set; }

            public string? ProfilePicture { get; set; }


       
            public IFormFile ProfilePictureFile { get; set; }
        }
    }

 
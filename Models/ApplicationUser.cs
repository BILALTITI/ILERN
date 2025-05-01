using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assigment2.Models
{
    public class ApplicationUser : IdentityUser
    {

 
            [Required]
            [StringLength(50)]
            public string FullName { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Required]
            public string Gender { get; set; }

            [Required]
            public string Address { get; set; }

            public string ProfilePicture { get; set; }   

            [Required]
            public string Role { get; set; }
   public bool     IsActive { get; set; }
    }
    }

 
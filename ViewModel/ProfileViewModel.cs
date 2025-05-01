using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment2.ViewModel
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ProfilePicture { get; set; }
        public IFormFile ProfilePictureFile { get; set; }
        // In ProfileViewModel
        public List<string> Roles { get; set; } = new List<string>(); // Initialize to avoid nulls}
        public bool  IsActive { get; set; }   
    }

}

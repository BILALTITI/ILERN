using System.ComponentModel.DataAnnotations;

namespace Assigment2.ViewModel
{
    public class LoginUserViewModel
    {
        //[Required]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

    }
}

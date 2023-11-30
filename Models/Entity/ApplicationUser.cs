using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.Models.Entity
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="First Name Is Requird ")]
        [MaxLength(50,ErrorMessage ="First Name Mustbe Less Than 50 Char ")]
        public string FirstName  { get; set; }
        [Required(ErrorMessage = "Last Name Is Requird ")]
        [MaxLength(50, ErrorMessage = "Last Name Mustbe Less Than 50 Char ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Adress  Is Requird ")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Phone Number  Is Requird ")]
        [RegularExpression(@"^\d{11}$", ErrorMessage ="phone Number must ")]
        public string Phone { get; set; }
    }
}

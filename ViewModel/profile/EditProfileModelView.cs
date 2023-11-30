using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.profile
{
    public class EditProfileModelView
    {
        [Display (Name ="First Name")]
        [Required(ErrorMessage = "First Name Is Requird ")]
        [MaxLength(50, ErrorMessage = "First Name Mustbe Less Than 50 Char ")]
        public string firstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Is Requird ")]
        [MaxLength(50, ErrorMessage = "Last Name Mustbe Less Than 50 Char ")]
        public string lastName { get; set; }

        [Required(ErrorMessage = "Address Is Requird ")]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required(ErrorMessage = "Phone Number  Is Requird ")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "phone Number must Contain 11 numbers")]
        [Display(Name = "Phone")]
        public string phone { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }




    }
}

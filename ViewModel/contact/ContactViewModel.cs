using System.ComponentModel.DataAnnotations;

namespace GraduaitionProjectITI.ViewModel.contact
{
    public class ContactViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name Is Requird ")]
        [MaxLength(50, ErrorMessage = "Name Must be Less Than 50 Chars ")]
        [MinLength(3, ErrorMessage = "Name should be at least 3 Chars ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone Number  Is Requird ")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "phone Number must Number And 11 Digit ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Branch Is Requird ")]
        [MaxLength(50, ErrorMessage = "Branch Must be Less Than 50 Char ")]
        [MinLength(3, ErrorMessage = "Branch should be at least 3 Char ")]
        public string Branch { get; set; }
        [Display(Name = "Message")]
        [MaxLength(200, ErrorMessage = "Message Must Be Less Than 200 Char ")]
        [MinLength(10, ErrorMessage = "Message Must Be Greater Than 10 Char")]
        public string Message { get; set; }
    }
}

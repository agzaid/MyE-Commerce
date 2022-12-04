using Data.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.UserAuthentication
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            GenderList = Enum.GetValues(typeof(Gender)).Cast<Gender>().Select(s => new SelectListItem()
            {
                Text = s.ToString(),
                Value = ((int)s).ToString(),
            }).ToList();
        }


        [Required(ErrorMessage = "first name is required!")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Characters only")]
        [StringLength(8, ErrorMessage = "Minimum {2} characters long and max of {1}", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "second name is required!")]
        [StringLength(8, ErrorMessage = "Minimum {2} characters long and max of {1}", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required!")]
        [Range(10, 120)]
        public int Age { get; set; }

        [Display(Name = "Primary Phone Number")]
        [Required(ErrorMessage = "phone number is required!")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Phone number is not valid.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Second Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Entered phone number is not valid.")]
        public string SecondPhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is missing or invalid.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Not Matched")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        public List<SelectListItem> GenderList { get; set; } = new List<SelectListItem>();

    }
}

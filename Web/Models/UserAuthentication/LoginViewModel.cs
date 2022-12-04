using System.ComponentModel.DataAnnotations;

namespace Web.Models.UserAuthentication
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="User name must be provided.Not Email !")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}

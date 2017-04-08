using System.ComponentModel.DataAnnotations;

namespace DreamCMS.ModelsAdmin
{
    public class LoginModel
    {
        public string ErrorMsg { get; set; }

        [Required(ErrorMessage="{0} is required.")]
        public string Username { get; set; }
        
        [Required(ErrorMessage="{0} is required.")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordModel
    {
        public string ErrorMsg { get; set; }

        [Required]
        [EmailAddress(ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }
    }

    public class LoginOrForgotPasswordModel
    {
        public LoginModel LoginModel {get;set;}
        public ForgotPasswordModel ForgotPasswordModel { get; set; }
    }

}
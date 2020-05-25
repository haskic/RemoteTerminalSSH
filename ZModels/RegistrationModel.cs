using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZModels
{
    public class RegistrationModel
    {
        [Compare("PasswordConfirm", ErrorMessage = "Passwords must be the same!")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "You should confirm password")]
        [Compare("Password", ErrorMessage = "Passwords must be the same!")]
        public string  PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "NickName is required")]
        public string NickName { get; set; }

    }
}

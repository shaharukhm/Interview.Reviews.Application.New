using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Library
{
   public class LoginModel
    {
        public int LoginId { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UpdatePasswordModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="New Password is required")]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
       
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The New Password and Confirm Password password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

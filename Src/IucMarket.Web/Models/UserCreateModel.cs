using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class UserCreateModel
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Full name")]
        [StringLength(100)]
        public string Fullname { get; set; }
    }
}

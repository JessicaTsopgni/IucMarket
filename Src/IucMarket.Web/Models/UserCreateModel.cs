using ExpressiveAnnotations.Attributes;
using IucMarket.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IucMarket.Web.Models
{
    public class UserCreateModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [RequiredIf("Id == null", AllowEmptyStrings = false, ErrorMessage = "{0} is required")]
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

        [Required]
        [Display(Name = "Phone code")]
        public string PhoneCountryCode { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public long? PhoneNumber { get; set; }

        public RoleOptions Role { get; set; }
        public bool Status { get; set; }

        public UserCreateModel()
        {
            Status = true;
            PhoneCountryCode = "+237";
        }

        public UserCreateModel(string id, string email, string password, 
            string confirmPassword, string fullname, string phoneCountryCode, long? phoneNumber,
            RoleOptions role, bool status):this()
        {
            Id = id;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            Fullname = fullname;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            Role = role;
            Status = status;
        }

        public override bool Equals(object obj)
        {
            return obj is UserCreateModel model &&
                   Id == model.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }

    
}

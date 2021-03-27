using IucMarket.Common;
using System;

namespace IucMarket.Web.Models
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneCountryCode { get; set; }
        public long Phonenumber { get; set; }
        public string FullPhoneNumber => $"{PhoneCountryCode} {Phonenumber}";
        public string RegistrationNumber { get; set; }
        public string Fullname { get; set; }
        public RoleOptions Role { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public UserListModel()
        {

        }

        public UserListModel(string id, string email, string phoneCountryCode, long phoneNumber, string registrationNumber, string fullname, DateTime createdDate,
            RoleOptions role, bool status)
        {
            Id = id;
            Email = email;
            PhoneCountryCode = phoneCountryCode;
            Phonenumber = phoneNumber;
            RegistrationNumber = registrationNumber;
            Fullname = fullname;
            CreatedDate = createdDate;
            Role = role;
            Status = status ? "Enabled" : "Disabled";
        }
    }
}

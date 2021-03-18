using System;

namespace IucMarket.Entities
{
    public class Person
    {
        public enum RoleOptions
        {
            Admin,
            Other
        }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneCountryCode { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleOptions Role { get; set; }
        public bool Status { get; set; }
        public Person()
        {

        }

        public Person(string userId, string fullName, string phoneCountryCode, long phoneNumber,
            DateTime createdAt, RoleOptions role, bool status)
        {
            UserId = userId;
            FullName = fullName;
            PhoneCountryCode = string.IsNullOrEmpty(phoneCountryCode) ? "+237" : phoneCountryCode;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            Role = role;
            Status = status;
        }
    }
}
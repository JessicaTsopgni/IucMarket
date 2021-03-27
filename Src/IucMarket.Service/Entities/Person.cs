using IucMarket.Common;
using System;
using System.Collections.Generic;

namespace IucMarket.Service.Entities
{
    internal class Person
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string RegistrationNumber { get; set; }
        public string PhoneCountryCode { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleOptions Role { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool Status { get; set; }
        public IEnumerable<Interaction> ProductInteractions { get; set; }

        public Person()
        {

        }

        public Person(string userId, string fullName, string registrationNumber, string phoneCountryCode, long phoneNumber,
            DateTime createdAt, RoleOptions role, bool isEmailVerified, bool status):this()
        {
            UserId = userId;
            FullName = fullName;
            RegistrationNumber = registrationNumber;
            PhoneCountryCode = string.IsNullOrEmpty(phoneCountryCode) ? "+237" : phoneCountryCode;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            Role = role;
            IsEmailVerified = isEmailVerified;
            Status = status;
        }

        public Person(string userId, string fullName, string registrationNumber, string phoneCountryCode, long phoneNumber,
           DateTime createdAt, RoleOptions role, bool isEmailVerified,
           bool status, IEnumerable<Interaction> productInteractions)
            :this(userId, fullName, registrationNumber, phoneCountryCode,
                 phoneNumber, createdAt, role, isEmailVerified, status)
        {
            ProductInteractions = productInteractions;
        }
    }
}
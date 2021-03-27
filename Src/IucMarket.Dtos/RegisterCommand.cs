
using IucMarket.Common;
using System;

namespace IucMarket.Dtos
{
    public class RegisterCommand
    {
        public string Email { get; private set; }
        public string RegistrationNumber { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public string PhoneCountryCode { get; private set; }
        public long PhoneNumber { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool SendVerificationEmail { get; private set; }
        public RoleOptions Role { get; private set; }
        public bool Status { get; private set; }

        public RegisterCommand(string email, string registrationNumber, string password, string name, string phoneCountryCode, long phoneNumber,
            bool sendVerificationEmail, RoleOptions role, bool status)
        {
            Email = email;
            RegistrationNumber = registrationNumber;
            Password = password;
            Name = name;
            PhoneCountryCode = phoneCountryCode;
            PhoneNumber = phoneNumber;
            CreatedAt =DateTime.UtcNow.AddHours(1);
            SendVerificationEmail = sendVerificationEmail;
            Role = role;
            Status = status;
        }
    }

}
